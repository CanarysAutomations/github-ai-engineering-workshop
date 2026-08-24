---
name: product-reviews
description: "Product reviews flow: open app, select product, view/add reviews, submit rating and comment."
argument-hint: "product reviews or add product review"
---

Steps:
1. Open the app UI at http://localhost:5173
2. Navigate to Catalog page
3. Select a product card and click product name or "View Details"
4. Scroll to Reviews section
5. View existing reviews (if any) with ratings and comments
6. If adding review go to orders section and select a product you have purchased:
   - Scroll to "Add Review" form
   - Select rating (1-5 stars)
   - Enter comment text (min 10 characters)
   - Click "Submit Review"
7. Verify review appears in list with your rating and comment
8. Verify average rating updates on product card

Quality: Review submission shows success message, review appears in reviews list, product rating badge updates
