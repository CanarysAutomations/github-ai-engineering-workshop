import { Routes, Route } from "react-router-dom";
import { Header } from "./components/Header";
import { CatalogPage } from "./pages/CatalogPage";
import { ProductDetailPage } from "./pages/ProductDetailPage";
import { CartPage } from "./pages/CartPage";
import { CheckoutPage } from "./pages/CheckoutPage";
import { OrderConfirmationPage } from "./pages/OrderConfirmationPage";
import { OrderHistoryPage } from "./pages/OrderHistoryPage";
import { LoginPage } from "./pages/LoginPage";
import "./App.css";

function App() {
  return (
    <div className="fk-app">
      <Header />
      <main className="fk-main">
        <Routes>
          <Route path="/" element={<CatalogPage />} />
          <Route path="/products/:id" element={<ProductDetailPage />} />
          <Route path="/cart" element={<CartPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/checkout" element={<CheckoutPage />} />
          <Route path="/order-confirmation/:orderId" element={<OrderConfirmationPage />} />
          <Route path="/orders" element={<OrderHistoryPage />} />
        </Routes>
      </main>
      <footer className="fk-footer">
        &copy; {new Date().getFullYear()} eShop — Demo e-commerce platform built with .NET &amp; React
      </footer>
    </div>
  );
}

export default App;
