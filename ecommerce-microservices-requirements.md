# E-Commerce Demo Application — Requirements Specification
### .NET Microservices Architecture

---

## 1. Purpose & Scope

Build a **demo-grade** e-commerce application that showcases a microservices architecture using .NET. The application demonstrates four core capabilities:

1. Adding items to a product catalog
2. Displaying the catalog to shoppers (no login required)
3. Adding items to a shopping cart (no login required — guest browsing/cart)
4. Checking out (placing an order) — **login required at this step only**

This is intended as a reference/demo build — scoped for clarity and speed of implementation (ideal for AI-assisted "vibe coding"), not for production hardening.

**Key constraints for this build:**
- **No database.** All data (catalog, cart, users, orders) is held **in memory** within each service (e.g., a static/singleton in-memory collection or `ConcurrentDictionary`). Data resets when a service restarts — this is expected and acceptable for a demo.
- **Login is required only at checkout**, not for browsing the catalog or building a cart. Catalog browsing and cart management work as a guest (identified by a session/guest ID).
- **Auth is simple username/password** — no external identity provider, no OAuth, no email verification. A small in-memory user list is sufficient.

---

## 2. Architecture Overview

### 2.1 Style
Microservices, each independently deployable, each owning its **own in-memory data store** (no external database — plain in-process collections, e.g. `ConcurrentDictionary<Guid, T>` or `List<T>` behind a singleton repository/service), communicating via:
- **Synchronous**: REST/HTTP (via an API Gateway) for query/command calls from the client
- **Asynchronous** *(optional)*: Lightweight message bus (RabbitMQ) for cross-service events (e.g., `OrderPlaced`, `CartCheckedOut`) — for a pure in-memory demo, a direct HTTP call between services is also acceptable and simpler to run without extra infrastructure

> Since there's no database, there are no EF Core migrations, no connection strings, and no persistence beyond process lifetime. Each service simply seeds its in-memory store on startup (e.g., sample products, one or two demo users).

### 2.2 Services

| # | Service | Responsibility |
|---|---------|-----------------|
| 1 | **Catalog Service** | Manage products (CRUD), categories, pricing, stock; serve catalog listing/detail |
| 2 | **Cart Service** | Manage per-user shopping cart (add/update/remove items, view cart) |
| 3 | **Order Service** | Handle checkout: create order from cart, calculate totals, simulate payment, persist order |
| 4 | **API Gateway** | Single entry point for the client (frontend); routes requests to backend services |
| 5 | **Identity Service** | Simple username/password login against an **in-memory user list**; issues a JWT (or simple token) used to authenticate at checkout only |

> Identity Service is **required** in this version (login is a first-class step at checkout), but stays intentionally minimal: a small in-memory list of seeded users (e.g., `demo/demo123`), a `/login` endpoint that validates credentials and returns a token, and a `/register` endpoint if you want users to be able to sign up on the fly (optional). No password hashing complexity is required for a demo, though a basic hash (e.g., BCrypt) is a nice touch if time permits.

### 2.3 High-Level Diagram (textual)

```
                     ┌───────────────┐
                     │   Client UI   │  (Blazor / React / Angular / Postman for demo)
                     └───────┬───────┘
                             │ HTTPS
                     ┌───────▼────────┐
                     │  API Gateway    │  (YARP or Ocelot)
                     └───┬───┬───┬───┬─┘
        ┌────────────────┘   │   │   └────────────────┐
┌───────▼──────┐  ┌──────────▼───┐ ┌▼─────────────┐ ┌──▼────────────┐
│ Catalog Svc   │  │  Cart Svc    │ │ Order Svc    │ │ Identity Svc  │
│ (in-memory)   │  │ (in-memory)  │ │ (in-memory)  │ │ (in-memory)   │
│ no login req. │  │ no login req.│ │ login req.   │ │ login/register│
└───────┬───────┘  └──────┬───────┘ └──────┬───────┘ └───────────────┘
        │                 │                │
        └───────────► (optional) Message Bus / or direct HTTP calls ◄────┘
```

Notes:
- Catalog & Cart services are reachable **without** a token (guest browsing/cart building).
- Order Service (checkout) **requires** a valid token issued by Identity Service — the Gateway (or Order Service itself) validates the JWT before allowing `/checkout`.
- All four services hold their state purely in memory (no SQL Server/PostgreSQL/SQLite, no EF Core).

---

## 3. Functional Requirements

### 3.1 Catalog Management (Catalog Service)
- FR1: Admin/user can **add a new product** with: name, description, price, category, SKU, stock quantity, image URL.
- FR2: Admin/user can **update** an existing product's details/stock.
- FR3: Admin/user can **delete/deactivate** a product.
- FR4: System can **list all products** (paginated), with filter by category and search by name.
- FR5: System can **retrieve a single product's details** by ID.
- FR6: Each product must show real-time-ish stock availability (or "in stock"/"out of stock" flag).

### 3.2 Catalog Display (Client-facing)
- FR7: User can browse the full catalog (grid/list view).
- FR8: User can view product details (image, description, price, availability).
- FR9: User can search/filter products by name/category.

### 3.3 Shopping Cart (Cart Service) — No Login Required
- FR10: User (as a **guest**, identified by a generated `guestId`/session ID — e.g., a GUID stored client-side, no login) can **add a product to cart** with a quantity.
- FR11: User can **view current cart** contents (items, quantity, unit price, line total, cart total) — without logging in.
- FR12: User can **update quantity** of an item in the cart.
- FR13: User can **remove an item** from the cart.
- FR14: Cart should validate against available stock (soft check by calling Catalog Service, or via cached price/stock snapshot).
- FR15: Cart is keyed by `guestId` and held in memory. It persists only for the lifetime of the service process/session (no login needed to build a cart).

### 3.4 Login (Identity Service) — Required Only at Checkout
- FR16: User can **log in** with a username and password against an **in-memory user list** (a few users seeded at startup, e.g. `demo/demo123`).
- FR17: On successful login, the Identity Service issues a **token** (simple JWT is recommended, but a plain opaque token + in-memory session map is acceptable for a pure demo).
- FR18: *(Optional)* User can **register** a new username/password, added to the in-memory user list at runtime.
- FR19: Invalid credentials return a clear `401 Unauthorized` with an appropriate message.

### 3.5 Checkout (Order Service) — Login Required
- FR20: When the user clicks **"Checkout"**, the system checks whether the user is logged in:
  - If **not logged in** → prompt for login (username/password) first.
  - If **logged in** (valid token present) → proceed directly to checkout.
- FR21: Once authenticated, the user's **existing guest cart is carried over** — i.e., checkout uses the same cart the guest was building (looked up by `guestId`, now associated with the logged-in `userId`).
- FR22: System calculates order total (sum of line items + optional tax/shipping — can be flat/mock values for demo).
- FR23: System captures a **shipping address** (simple form: name, address, city, zip — no real validation needed).
- FR24: System simulates **payment confirmation** (mock/always-success payment step — no real gateway integration).
- FR25: On successful checkout:
  - An **Order** record is created (status: `Placed`) in Order Service's in-memory store, tied to `userId`.
  - Cart is cleared in Cart Service (via direct call or optional event).
  - Stock is decremented in Catalog Service's in-memory store (via direct call or optional event).
- FR26: User can **view their order history** and **order detail/status** (requires login, since orders are tied to `userId`).

### 3.6 Cross-Cutting
- FR27: Requests to Catalog and Cart endpoints do **not** require a token. Requests to Order (checkout, order history) **do** require a valid token — enforced via `[Authorize]` on the Order Service (and/or at the Gateway).
- FR28: Each service exposes a **Swagger/OpenAPI** page for demo/testing.
- FR29: Centralized logging (console output is sufficient for demo).
- FR30: All state (products, carts, users, orders) lives **only in memory** — clearly document that restarting a service clears its data, and seed reasonable demo data on startup for a smooth demo.

---

## 4. Non-Functional Requirements (Demo-Scoped)

| Category | Requirement |
|---|---|
| **Simplicity** | Prioritize working end-to-end flow over production polish |
| **Containerization** | Each service runs in its own Docker container; `docker-compose` to spin up entire stack locally |
| **Data stores** | Lightweight — SQLite/PostgreSQL/SQL Server per service (or shared dev SQL Server with separate schemas as a shortcut) |
| **Resilience** | Basic retry/circuit breaker on inter-service HTTP calls (e.g., Polly) — nice to have, not mandatory |
| **Security** | Minimal: JWT-based auth (or none, using a mock user header) — no need for full identity provider |
| **Performance** | Not a concern for demo scale |
| **Observability** | Console logging + Swagger is sufficient; optional: basic health checks (`/health` endpoint per service) |
| **Testability** | Should be runnable locally with one command (`docker-compose up`) and demoable via UI or Postman collection |

---

## 5. Suggested Tech Stack

| Layer | Technology |
|---|---|
| Language/Runtime | .NET 8 (C#) |
| Service Framework | ASP.NET Core Web API (Minimal APIs or Controllers) |
| API Gateway | YARP (Yet Another Reverse Proxy) or Ocelot |
| Data Storage | **In-memory only** — `ConcurrentDictionary<Guid, T>` / `List<T>` wrapped in a singleton repository class per service. **No EF Core, no SQL Server/PostgreSQL/SQLite, no database of any kind.** |
| Messaging | *(Optional)* RabbitMQ + MassTransit for `OrderPlaced`/`CartCleared`/`StockDecremented` events — direct HTTP calls between services are a simpler, equally valid alternative for a pure in-memory demo |
| Auth | Simple username/password login issuing a JWT (`System.IdentityModel.Tokens.Jwt`), validated via standard ASP.NET Core JWT Bearer middleware — no ASP.NET Core Identity, no external provider |
| Containerization | Docker + Docker Compose *(optional — since there's no DB to containerize, running services directly via `dotnet run` is also simple enough for a demo)* |
| Frontend (optional) | Blazor Server/WASM, or a simple React/Angular SPA, or just Swagger/Postman for a backend-only demo |
| API Docs | Swashbuckle (Swagger/OpenAPI) per service |
| Resilience (optional) | Polly for retries/circuit breakers |

---

## 6. Data Model (Simplified — all held in-memory, no DB)

> Each service keeps its own collection in memory, e.g.:
> `private static readonly ConcurrentDictionary<Guid, Product> _products = new();`
> seeded with a few sample rows in `Program.cs` / a startup seeding method.

### Catalog Service — `Product`
```
Id (Guid)
Name (string)
Description (string)
Category (string)
Price (decimal)
Sku (string)
StockQuantity (int)
ImageUrl (string)
IsActive (bool)
CreatedAt / UpdatedAt (DateTime)
```

### Cart Service — `Cart` / `CartItem`  (keyed by guest ID; no login needed)
```
Cart:
  Id (Guid)
  GuestId (string)   -- generated client-side (e.g., GUID in local storage/cookie); becomes UserId after login at checkout
  CreatedAt / UpdatedAt

CartItem:
  Id (Guid)
  CartId (Guid, FK)
  ProductId (Guid)
  ProductName (string) -- denormalized snapshot
  UnitPrice (decimal)  -- snapshot at time of add
  Quantity (int)
```

### Identity Service — `User`  (in-memory, seeded at startup)
```
User:
  Id (Guid)
  Username (string)   -- e.g., "demo"
  PasswordHash (string) -- plain-text acceptable for pure demo; basic hash (BCrypt) recommended if time permits
  CreatedAt
```

### Order Service — `Order` / `OrderItem`  (requires an authenticated `UserId`)
```
Order:
  Id (Guid)
  UserId (string)    -- from the validated token/login, not a guest ID
  Status (enum: Placed, Confirmed, Shipped, Cancelled)
  ShippingAddress (string / object)
  TotalAmount (decimal)
  CreatedAt

OrderItem:
  Id (Guid)
  OrderId (Guid, FK)
  ProductId (Guid)
  ProductName (string)
  UnitPrice (decimal)
  Quantity (int)
  LineTotal (decimal)
```

---

## 7. Key API Endpoints (via Gateway)

### Catalog Service — 🔓 No login required
```
POST   /api/catalog/products          - Add product
GET    /api/catalog/products          - List/search products (paged)
GET    /api/catalog/products/{id}     - Get product detail
PUT    /api/catalog/products/{id}     - Update product
DELETE /api/catalog/products/{id}     - Delete/deactivate product
```

### Cart Service — 🔓 No login required (guest-based)
```
GET    /api/cart/{guestId}                     - Get current cart
POST   /api/cart/{guestId}/items               - Add item to cart
PUT    /api/cart/{guestId}/items/{itemId}      - Update item quantity
DELETE /api/cart/{guestId}/items/{itemId}      - Remove item
DELETE /api/cart/{guestId}                     - Clear cart
```

### Identity Service — 🔓 No login required to call these (they *are* the login)
```
POST   /api/identity/login            - Validate username/password, return token
POST   /api/identity/register         - (Optional) create a new in-memory user
```

### Order Service — 🔒 Login required (valid Bearer token)
```
POST   /api/orders/checkout           - Create order from guest's cart (checkout) — requires Authorization header
GET    /api/orders                    - List order history for the logged-in user (userId taken from token)
GET    /api/orders/{orderId}          - Get order detail (must belong to the logged-in user)
```

> **Checkout flow contract:** `POST /api/orders/checkout` accepts `{ guestId, shippingAddress }` in the body plus a `Bearer <token>` header. Order Service resolves `userId` from the token, fetches the cart via `guestId` from Cart Service, creates the order, then tells Cart Service to clear that cart and Catalog Service to decrement stock.

---

## 8. Event Contracts (Optional — Async via RabbitMQ, or replace with direct HTTP calls)

> Since everything is in-memory and this is a demo, **direct synchronous HTTP calls** from Order Service to Cart Service and Catalog Service (e.g., `PUT /api/catalog/products/{id}/decrement-stock`, `DELETE /api/cart/{guestId}`) are perfectly acceptable and simpler to stand up than a full message bus. Use the event-based approach below only if you specifically want to demonstrate async messaging.

| Event | Published By | Consumed By | Purpose |
|---|---|---|---|
| `OrderPlaced` | Order Service | Catalog Service, Cart Service | Decrement stock; clear cart |
| `StockDecremented` (optional ack) | Catalog Service | Order Service | Confirm stock update succeeded |

---

## 9. Demo Flow (User Journey to Script/Test)

1. Products and demo users are **seeded in memory** on service startup (5–10 sample products; 1–2 demo users like `demo/demo123`).
2. User (**as a guest, no login**) opens the storefront and browses/searches the catalog.
3. User views a product detail page.
4. User (**still as a guest**) adds 2–3 products to the cart with varying quantities.
5. User views the cart and adjusts quantity/removes an item — all without logging in.
6. User clicks **"Checkout"** → system detects no active login → shows a **login form**.
7. User logs in with username/password → Identity Service validates and returns a token.
8. User is returned to checkout with their existing cart intact → enters shipping info → confirms (mock) payment.
9. Order is placed → cart is cleared → stock is decremented.
10. User views order confirmation and order history (now accessible since they're logged in).

---

## 10. Suggested Build Sequence (for AI-assisted "vibe coding")

1. **Scaffold solution structure** — one .NET solution, one project per service (Catalog, Cart, Identity, Order) + gateway, shared contracts/DTOs project.
2. **Catalog Service** — in-memory `Product` store (`ConcurrentDictionary`), seed data, CRUD endpoints, Swagger. No login required.
3. **Cart Service** — in-memory `Cart`/`CartItem` store keyed by `guestId`, endpoints; calls Catalog Service for price/stock validation. No login required.
4. **Identity Service** — in-memory seeded `User` list, `/login` (and optional `/register`) endpoint issuing a JWT.
5. **Order Service** — in-memory `Order`/`OrderItem` store; `[Authorize]`-protected checkout endpoint that validates the JWT from Identity Service, resolves the guest's cart, creates the order, then calls Cart Service (clear cart) and Catalog Service (decrement stock) directly via HTTP.
6. **API Gateway** — configure routes to all four services (YARP/Ocelot); ensure only Order Service routes require the Bearer token (Catalog/Cart routes stay open).
7. **JWT wiring** — configure Identity Service to issue tokens and Order Service to validate them (shared signing key/config for the demo).
8. *(Optional)* **Messaging** — only if you want to demonstrate RabbitMQ/MassTransit instead of direct HTTP calls between Order → Cart/Catalog.
9. **Frontend (optional)** — simple SPA or Blazor app: catalog + cart pages work without login; checkout page triggers a login prompt if no token is present, then continues checkout.
10. **Smoke test** — run through the full demo flow (Section 9) end-to-end, confirming: browsing/cart work without login, checkout blocks until login, and cart/stock update correctly after order placement.

---

## 11. Out of Scope (for this demo)

- Any database or persistence layer (SQL Server, PostgreSQL, SQLite, NoSQL, files) — **everything is in-memory by design**
- Real payment gateway integration
- Password reset, email verification, multi-factor auth
- Full identity provider (Azure AD B2C, IdentityServer, Auth0, etc.) — a hand-rolled in-memory login is intentional here
- Multi-currency / multi-region support
- Advanced search (Elasticsearch), recommendations, reviews/ratings
- CI/CD pipeline, Kubernetes deployment
- Comprehensive automated test suite (unit tests are a nice-to-have, not required)
- Data durability across restarts (restarting any service is expected to reset its in-memory state — call this out explicitly during the demo)

---

## 12. Deliverables Checklist

- [ ] Solution with Catalog, Cart, Identity, Order services + API Gateway
- [ ] In-memory data stores (no DB) for all four services, with seed data on startup
- [ ] REST endpoints per service (Section 7), with Catalog/Cart open and Order protected by JWT
- [ ] Identity Service `/login` (and optional `/register`) issuing a working JWT
- [ ] Order Service validating the JWT and rejecting unauthenticated checkout attempts
- [ ] (Optional) RabbitMQ/MassTransit event wiring (Section 8) — or simple direct HTTP calls between services
- [ ] Swagger UI enabled per service
- [ ] Seed data for demo products and demo user(s)
- [ ] (Optional) Simple frontend or Postman collection demoing: guest browse → guest cart → login-gated checkout
