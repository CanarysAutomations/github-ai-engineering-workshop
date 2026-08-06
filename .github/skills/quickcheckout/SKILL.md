---
name: quickcheckout
description: "Quick checkout flow: open app, select product, buy now, login (demo), enter address, confirm order."
argument-hint: "quick checkout or buy demo product"
---

Steps:
1. Open the app UI at http://localhost:5173
2. Select a product card and click "Buy Now"
3. If prompted, sign in: user `demo` / pass `demo123`
4. Fill shipping fields (Name, Street, City, ZIP, Country)
5. Confirm payment info and click "Place Order"
6. Verify order confirmation (order id or success message)
Quality: Order confirmation page shows success with order id