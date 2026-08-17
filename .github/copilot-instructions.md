# GitHub Copilot Instructions — eShop Repository

1. **Microservices** — Each service owns its domain. Minimal APIs only; no Controllers or cross-service direct calls. Accept caller-supplied data as-is.
2. **In-Memory Data** — `ConcurrentDictionary<TKey, T>` for all storage, Singleton, seeded on startup.
3. **Shared Contracts** — All DTOs in `MyEcomm.Contracts`. Never remove/rename/change existing DTO properties; optional additions OK.
4. **JWT Auth** — Identity Service issues; services validate locally. Protected endpoints return 401 if invalid.
5. **RESTful APIs** — Minimal APIs with `MapGroup()`. Return DTOs. Status codes: 201 (create), 200 (success), 204 (delete), 400 (validation), 404 (not found), 401 (auth), 409 (conflict).
6. **YARP Gateway** — All frontend calls via gateway (:5100). No direct service calls from client.
7. **Concurrency** — Use ConcurrentDictionary atomic methods (TryAdd, TryUpdate, TryRemove). If TryUpdate fails, retry up to 3 times before returning 409 Conflict.
8. **Swagger + CORS** — Enable `/swagger` per service. CORS: `http://localhost:5173` and `http://localhost:5100`.
9. **Frontend UI** — All frontend work MUST follow the rules in [`.github/instructions/ui.instructions.md`](.github/instructions/ui.instructions.md) exactly.

