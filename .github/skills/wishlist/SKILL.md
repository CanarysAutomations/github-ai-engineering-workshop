---
name: wishlist
description: "Wishlist flow: open app, add/remove products to wishlist, view wishlist, move to cart."
argument-hint: "wishlist or manage wishlist"
---

Steps:
1. Open the app UI at http://localhost:5173
2. Sign in: user `demo` / pass `demo123`
3. Navigate to Catalog page
4. Add product to cart:
   - Click on save to wishlist action on cart page
5. Click "Wishlist" link in header or navigation
6. Verify added products appear in Wishlist page
7. On Wishlist page:
   - Verify product details (name, price, image)
   - Click "Move to Cart" or "Add to Cart" button for any item
   - Verify item moved to cart (wishlist count decreases)
8. Remove from wishlist:
   - Click remove icon on wishlist item
   - Verify item removed from list
9. Verify empty wishlist shows "Your wishlist is empty" message

Quality: Wishlist persists across navigation, heart icon state updates correctly, move-to-cart functionality works, empty state displays
