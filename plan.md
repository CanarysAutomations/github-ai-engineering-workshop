# MyEcomm — .NET Microservices E-Commerce Demo — Implementation Plan

## Context

The project directory currently contains only `ecommerce-microservices-requirements.md` — this is a greenfield build with no existing code. The requirements spec calls for a demo-grade e-commerce app built with four .NET microservices (Catalog, Cart, Identity, Order) plus an API Gateway, all using **in-memory data only** (no database anywhere), with login required only at checkout. The goal is a runnable, demoable system that proves out: guest browsing/cart building without login, a login gate that triggers exactly at checkout, and a full checkout flow that clears the cart and decrements stock.

Confirmed build decisions (from user):
- **.NET 8 LTS**, ASP.NET Core Minimal APIs
- **YARP** gateway + **direct synchronous HTTP calls** between services (no RabbitMQ/MassTransit)
- **React SPA** (Vite + TypeScript) as the frontend
- **No Docker/docker-compose** — run everything via `dotnet run` / `npm run dev` locally

Multiple SDKs are installed on this machine (8.0.422, 9.0.308, 10.0.301) — a `global.json` pinning 8.0.422 is needed so `dotnet` commands don't roll forward.

---

## 1. Solution & Folder Structure

```
myecomm/
├── global.json                          (pins SDK to 8.0.422)
├── MyEcomm.sln
├── ecommerce-microservices-requirements.md
├── src/
│   ├── Services/
│   │   ├── Catalog/MyEcomm.Catalog.Api/
│   │   ├── Cart/MyEcomm.Cart.Api/
│   │   ├── Identity/MyEcomm.Identity.Api/
│   │   └── Order/MyEcomm.Order.Api/
│   ├── Gateway/MyEcomm.Gateway/
│   └── Shared/MyEcomm.Contracts/        (DTOs shared by all services + gateway)
└── client/                              (React + Vite + TS SPA)
```

Each service project follows the same internal layout: `Program.cs`, `appsettings.json`, `Endpoints/*.cs` (Minimal API route groups), `Models/*.cs`, `Repositories/{I*Repository.cs, InMemory*Repository.cs}`, and (where needed) `Clients/*ServiceClient.cs` and `Seed/*Seeder.cs`.

`MyEcomm.Contracts` is a plain class library (no ASP.NET dependency) holding all request/response DTOs (`ProductDto`, `CartDto`/`CartItemDto`, `LoginRequest`/`LoginResponse`, `CheckoutRequest`, `OrderDto`, etc.) — referenced by every service so the HTTP contracts have one source of truth.

Build with: `dotnet new sln`, `dotnet new classlib` for Contracts, `dotnet new web` for each service + gateway (Minimal APIs, not Controllers — keeps this demo-scoped build simple), then `dotnet sln add` each project.

---

## 2. Per-Service Design

All services register their in-memory store as a **Singleton** wrapping `ConcurrentDictionary<TKey, T>`, seeded on startup, with Swagger enabled (`Swashbuckle.AspNetCore` 6.6.2) per FR28.

### Catalog Service (`MyEcomm.Catalog.Api`, port 5101) — no auth
- `InMemoryProductRepository`: `ConcurrentDictionary<Guid, Product>`. Supports paged list w/ category filter + name search, get-by-id, add, update, soft-delete (`IsActive=false`), and a concurrency-safe `TryDecrementStock(productId, qty)` (compare-and-swap loop) for the internal decrement-stock call.
- Endpoints under `/api/catalog/products`: `POST`, `GET` (paged/filtered), `GET /{id}`, `PUT /{id}`, `DELETE /{id}`, plus `POST /{id}/decrement-stock` (internal, called by Order Service).
- Seed: 8–10 products across 2–3 categories, varied stock (including one out-of-stock item to exercise that UI state).

### Cart Service (`MyEcomm.Cart.Api`, port 5102) — no auth
- `InMemoryCartRepository`: `ConcurrentDictionary<string, Cart>` keyed by `guestId`; auto-creates an empty cart on first access (no explicit create-cart endpoint).
- `CatalogServiceClient` (named `HttpClient`, direct port `http://localhost:5101`): fetches price/stock snapshot when adding an item, soft-validates requested quantity against stock (FR14) — logs and proceeds if Catalog is unreachable rather than hard-failing (demo resilience choice).
- Endpoints under `/api/cart/{guestId}`: `GET`, `POST /items`, `PUT /items/{itemId}`, `DELETE /items/{itemId}`, `DELETE` (clear cart — also called by Order Service post-checkout).

### Identity Service (`MyEcomm.Identity.Api`, port 5103) — issues auth, not itself protected
- Packages: `System.IdentityModel.Tokens.Jwt` 8.0.2, `BCrypt.Net-Next` 4.0.3 (basic password hashing — cheap nice-to-have per spec's suggestion).
- `InMemoryUserRepository`: `ConcurrentDictionary<Guid, User>`, seeded with `demo/demo123` and `alice/alice123` (a second user demonstrates per-user order history isolation).
- `JwtTokenService`: builds a signed JWT with claims `sub`=userId, a username claim, `jti`, `iat`; reads issuer/audience/signing key/lifetime from config.
- Endpoints under `/api/identity`: `POST /login` (401 with clear message on bad credentials per FR19), `POST /register` (409 if username taken).

### Order Service (`MyEcomm.Order.Api`, port 5104) — **only protected service**
- Packages: `Microsoft.AspNetCore.Authentication.JwtBearer` 8.0.11.
- `InMemoryOrderRepository`: `ConcurrentDictionary<Guid, Order>`; `GetByUserId`/`GetById` enforce ownership (return 404, not 403, for orders belonging to another user).
- `CartServiceClient` + `CatalogServiceClient` (direct ports 5102/5101).
- `app.MapGroup("/api/orders").RequireAuthorization()` — every route in this service requires a valid Bearer token, validated against the same signing key Identity Service uses.
- `POST /api/orders/checkout`: body `{ guestId, shippingAddress }`; resolves `userId` from token claims; fetches cart via `guestId`; 400 if empty; builds order with snapshot line items + flat shipping constant; persists with `Status=Placed`; calls Catalog to decrement stock per line (logs failures without rolling back — acceptable for a demo, not a distributed transaction); calls Cart to clear; returns 201 + `OrderDto`.
- `GET /api/orders`, `GET /api/orders/{orderId}` — scoped to the authenticated user.

### API Gateway (`MyEcomm.Gateway`, port 5100)
- Package: `Yarp.ReverseProxy` 2.2.0. Pure reverse proxy — **no JWT validation at the gateway**; it forwards the `Authorization` header through unchanged and lets Order Service's own `[Authorize]`/`RequireAuthorization()` do the enforcement (FR27 allows either place; enforcing only in Order Service keeps the signing-key config in exactly two places — Identity + Order — instead of three, and matches the spec's demo-scoped simplicity priority).
- YARP route table maps `/api/catalog/**` → 5101, `/api/cart/**` → 5102, `/api/identity/**` → 5103, `/api/orders/**` → 5104.
- CORS policy `AllowFrontend` for `http://localhost:5173` — this is the only place CORS needs to be configured since the SPA only talks to the gateway.

---

## 3. JWT Configuration

Shared config block (duplicated identically in Identity's and Order's `appsettings.json` under a `"Jwt"` section — call this out as a demo shortcut in place of a shared secrets store):
```json
"Jwt": {
  "Issuer": "MyEcomm.Identity",
  "Audience": "MyEcomm.Client",
  "SigningKey": "<32+ byte shared secret>",
  "AccessTokenMinutes": 60
}
```
HMAC-SHA256 symmetric key. 60-minute token lifetime is generous enough to cover a full demo session without expiring mid-walkthrough. No refresh tokens (out of scope, matches spec).

---

## 4. Inter-Service HTTP Calls

Named `HttpClient`s via `IHttpClientFactory`, pointed at **direct service ports**, not routed back through the gateway (the gateway is a client-facing concern only; internal calls avoid an unnecessary hop and avoid coupling to gateway CORS/routing rules). Each client sets a 10s timeout so a hung downstream fails fast rather than hanging the UI. Polly retries are explicitly optional per spec — skip for the initial build.

---

## 5. React SPA (`client/`)

Scaffold via `npm create vite@latest client -- --template react-ts`. Add `react-router-dom` and `axios`.

- **`guestId`**: generated via `crypto.randomUUID()` on first load, persisted in `localStorage` so a guest's cart survives page reloads (server-side cart state is separately scoped to the Cart Service process per FR15).
- **JWT storage: in-memory only** (React context state, not `localStorage`). This is a deliberate choice so that every fresh page load starts as a guest and the checkout login-gate — the centerpiece of the demo flow — reliably reproduces on every run rather than being silently skipped by a persisted session. Trade-off (session lost on hard refresh) is expected/documented, not a bug.
- **Pages**: `CatalogPage` (grid + search/category filter), `ProductDetailPage`, `CartPage`, `CheckoutPage` (renders `LoginForm` inline when unauthenticated, falls through to `ShippingAddressForm` once a token exists — same `guestId` throughout, so "cart carries over" requires no special handling), `OrderConfirmationPage`, `OrderHistoryPage`.
- **API client**: single axios instance with base URL = gateway (`http://localhost:5100`); an `AuthContext`-driven helper attaches `Authorization: Bearer <token>` only when a token is present.
- Mock payment: a short client-side delay (~800ms) before calling checkout, to visually sell "processing payment" per FR24.

---

## 6. Build Order

1. `global.json` + `MyEcomm.sln` + `MyEcomm.Contracts` (DTOs first — everything else depends on it)
2. Catalog Service (fully verifiable standalone via Swagger)
3. Cart Service (verify against running Catalog, direct ports)
4. Identity Service (verify login/register via Swagger, decode JWT via jwt.io to sanity check claims)
5. Order Service (verify 401 without token, verify full checkout against direct ports before gateway exists)
6. API Gateway (repeat step 5's checks through port 5100 to confirm routing/header pass-through)
7. React SPA, built in user-journey order: Catalog → Cart → Checkout (login-gated) → Confirmation → Order History, wired to the gateway from the start
8. Full smoke test with all 6 processes running together

---

## 7. Verification (End-to-End)

Run each service (`dotnet run --project <path>` for the 4 services + gateway) plus `npm run dev` in `client/`. Confirm each backend's Swagger UI loads (ports 5101–5104) and the gateway proxies correctly (e.g. `GET http://localhost:5100/api/catalog/products` returns seeded data).

Manual smoke test (mirrors spec Section 9):
1. Browse catalog as guest, search/filter, view product detail — confirm no auth header sent on any of these calls.
2. Add multiple items to cart as guest, adjust quantity, remove an item — confirm totals recompute correctly.
3. Click Checkout → confirm a login form appears (not the shipping form).
4. Try a wrong password → confirm a clear 401-backed error message.
5. Log in with `demo/demo123` → confirm the shipping form now appears and the cart contents from step 2 are unchanged.
6. Submit shipping address → confirm mock payment processing → order confirmation with correct items/total.
7. Confirm cart is now empty, and the ordered product's stock has decreased (cross-check via Catalog Swagger).
8. View Order History → confirm the new order appears with status `Placed`; open its detail and confirm it matches.
9. Negative checks: call `GET /api/orders` via the gateway with no Bearer token → 401. Log in as `alice` and confirm her order history does not show `demo`'s order.
10. Restart one backend service mid-session and confirm its in-memory state resets — this demonstrates FR30's documented behavior, not a defect.

---

## Critical Files
- [ecommerce-microservices-requirements.md](ecommerce-microservices-requirements.md) — source of truth for all endpoint contracts/data models/demo flow
- `src/Shared/MyEcomm.Contracts/*.cs` — shared DTOs, build first
- `src/Services/Order/MyEcomm.Order.Api/Program.cs` + `Endpoints/OrderEndpoints.cs` — JWT validation + checkout orchestration
- `src/Services/Identity/MyEcomm.Identity.Api/Services/JwtTokenService.cs` — must stay in lockstep with Order Service's `Jwt` config
- `src/Gateway/MyEcomm.Gateway/appsettings.json` — YARP route/cluster table
- `client/src/context/AuthContext.tsx` + `client/src/pages/CheckoutPage.tsx` — in-memory token + login-gated checkout flow