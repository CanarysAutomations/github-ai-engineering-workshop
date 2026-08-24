# Exercise 03 — Plan: Design the Technical Architecture

> **Duration**: 8 minutes<br>
> **GitHub/Copilot Feature**: Spec Kit Plan<br>
> **Goal**: Translate the specification into a concrete technical design: API contracts, data models, service boundaries, and integration flows.

---

## Background

The **Plan** transforms the spec into concrete architecture: Service Boundaries, API Endpoints, Data Models, Integration Flows, Error Handling, and Concurrency Strategy.

---

## Step 1 — Run Spec Kit Plan

In Copilot Chat, paste this concise plan prompt:

```
/speckit.plan
Wishlist:
- Header: nav link alongside Cart using fk-cart-btn style WITH a count badge (fk-cart-count class, same as Cart badge) showing number of wishlist items.
- CartItemRow only: "SAVE TO WISHLIST" inline action button (fk-cart-item-remove style) placed after REMOVE. Clicking it MUST save to wishlist AND remove the item from the cart in a single action (wishlistApi.addItem + onRemove). Do NOT add any wishlist button to ProductDetailPage.
- WishlistPage (/wishlist): list layout (not grid) showing image, name, price, REMOVE per item. Page MUST include a "MOVE ALL TO CART" button that calls a backend MoveAllToCart endpoint (POST /api/wishlist/move-to-cart) which adds all wishlist items to cart via CartServiceClient and clears the wishlist on success.
- WishlistContext: a React context (WishlistContext.tsx) exposing itemCount and refreshWishlist, wrapping the app in main.tsx, used by Header for the count badge and by WishlistPage/CartItemRow after mutations.
Reviews: rating badge + review list on product detail page below description; review submission form on orders page per order item.


```

> **Tip**: Copilot references Constitution and Spec to generate an aligned plan.

![Spec Kit Plan](assets/wishlistplan.png)

---

## Step 2 — Review & Commit

Once Copilot completes, open `.specs/001-add-wishlist-reviews/plan.md`:

- Service boundaries clear
- All endpoints listed (HTTP verbs, paths, request/response)
- Data models minimal and clear
- Integration flows documented
- Error handling covers all status codes
- Concurrency strategy explicit

Commit:

```bash
git add .specs/001-add-wishlist-reviews/plan.md && git commit -m "spec: plan"
```

---

## Verify

- [ ] Architecture plan is documented and committed
- [ ] Service boundaries and endpoints are defined
- [ ] Integration flows and data models are clear

---

## Productivity Benefit

> A generated plan with endpoint contracts and data models cuts design meeting time by 60–80%. Developers start coding with a clear blueprint instead of discovering API shape mid-implementation and triggering late-stage rework.

---

**Next**: [Exercise 04 — Tasks: Break into Implementation Work Items](exercise-04-tasks.md)
