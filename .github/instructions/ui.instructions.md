---
description: Frontend UI rules for eShop. Apply when creating or editing React components, CSS, or any frontend feature.
applyTo: 'client/**'
---

# eShop Frontend UI Instructions — MUST follow exactly for every new feature

## CSS Class Naming
- ALL new CSS classes MUST use the `fk-` prefix (e.g. `fk-wishlist-btn`, `fk-review-card`).
- Never use Tailwind, Bootstrap, MUI, or any external component library classes.

## Design Tokens (never hardcode colors — use these exact values)
| Token | Value | Usage |
|---|---|---|
| Primary green | `#047857` | Header bg, primary buttons, links |
| CTA green | `#059669` | Buy Now, Place Order buttons |
| Badge green | `#10B981` | Ratings, success badges, discounts |
| Page bg | `#F8FAFC` | `.fk-main` background |
| Card bg | `#ffffff` | All cards/panels |
| Text | `#212121` | Body text |
| Muted text | `#878787` | Secondary labels, counts |
| Border | `#f0f0f0` | Dividers, card borders |
| Error red | `#ff6161` | Error states, cart badge |

## Button Styles
- **Primary button**: `background: #047857; color: #fff; border: none; border-radius: 2px; padding: 10px 24px; font-size: 14px; font-weight: 600; cursor: pointer;`
- **Secondary/outline button**: `background: #fff; color: #047857; border: 1px solid #047857; border-radius: 2px; padding: 10px 24px; font-size: 14px; font-weight: 600; cursor: pointer;`
- **Danger button**: `color: #ff6161; border-color: #ff6161;`
- **New header nav links**: use class `fk-cart-btn` — SVG icon + feature name as text label, white text, placed in header nav alongside existing links.
- **Inline action buttons in list rows** (e.g. secondary actions next to REMOVE): use class `fk-cart-item-remove` — same muted text style, placed inline with existing row actions.

## Component Rules
- Cards: `background: #fff; border-radius: 2px; box-shadow: 0 1px 2px rgba(0,0,0,.08);` — NEVER use rounded corners > 4px.
- Rating badge: `background: #10B981; color: #fff; font-size: 11px; font-weight: 700; padding: 2px 6px; border-radius: 2px;`
- Error message: `background: #fce4ec; border-left: 3px solid #e91e63; color: #c62828; padding: 10px 14px; border-radius: 2px;`
- Success toast: `background: #d1fae5; border-left: 3px solid #10B981; color: #059669; padding: 10px 14px; border-radius: 2px;`
- Page section max-width: `1280px`, centered with `margin: 0 auto; padding: 16px;`

## Font
- Font family: `'Roboto', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif`
- Base size: 14px; headings font-weight: 600.

## React Component Rules
- Use functional components with TypeScript (`.tsx`).
- Use existing CSS from `client/src/App.css` — add new classes to the SAME file, never create separate CSS files per feature.
- No inline `style={{}}` for colors or layout — use `className` with `fk-` classes only.
- Integrate new pages into `client/src/App.tsx` router using `<Route>`.
- Use `client/src/api/` pattern for all HTTP calls (see `cartApi.ts` as reference).
- Use `useAuth()` from `AuthContext` for auth state; `useCart()` from `CartContext` for cart.