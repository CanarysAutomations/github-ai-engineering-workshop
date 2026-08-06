import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuth } from "../context/AuthContext";
import { useCart } from "../context/CartContext";
import { ordersApi } from "../api/ordersApi";
import { LoginForm } from "../components/LoginForm";
import { ShippingAddressForm } from "../components/ShippingAddressForm";
import type { ShippingAddress } from "../types";

export function CheckoutPage() {
  const { isAuthenticated } = useAuth();
  const { guestId, cart, refreshCart, clearLocalCart } = useCart();
  const navigate = useNavigate();
  const [processing, setProcessing] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [loggedInJustNow, setLoggedInJustNow] = useState(false);

  async function handleCheckout(address: ShippingAddress) {
    setProcessing(true);
    setError(null);
    try {
      await new Promise((resolve) => setTimeout(resolve, 800));
      const order = await ordersApi.checkout(guestId, address);
      clearLocalCart();
      await refreshCart();
      navigate(`/order-confirmation/${order.id}`, { state: { order } });
    } catch (err) {
      if (axios.isAxiosError(err) && err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("Checkout failed. Please try again.");
      }
    } finally {
      setProcessing(false);
    }
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="fk-empty-cart">
        <span className="fk-empty-cart-icon">🛒</span>
        <h2>Your cart is empty!</h2>
        <p>Add items to it before checking out.</p>
        <button className="fk-btn-continue" onClick={() => navigate("/")}>
          Back to Catalog
        </button>
      </div>
    );
  }

  return (
    <div className="fk-checkout-layout">
      {/* Left column */}
      <div>
        {!isAuthenticated ? (
          <LoginForm onSuccess={() => setLoggedInJustNow(true)} />
        ) : (
          <>
            {loggedInJustNow && (
              <div className="fk-checkout-success">
                ✔ Logged in. Your cart has been carried over — proceed below.
              </div>
            )}
            <ShippingAddressForm onSubmit={handleCheckout} submitting={processing} />
            {error && <div className="fk-error-msg" style={{ marginTop: 12 }}>{error}</div>}
          </>
        )}
      </div>

      {/* Right column: order summary */}
      <div className="fk-price-summary">
        <div className="fk-price-summary-header">Order Summary</div>
        <div className="fk-price-summary-rows">
          {cart.items.map((item) => (
            <div key={item.id} className="fk-price-row" style={{ fontSize: 14 }}>
              <span style={{ maxWidth: 180, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                {item.productName} × {item.quantity}
              </span>
              <span>${item.lineTotal.toFixed(2)}</span>
            </div>
          ))}
          <div className="fk-price-row">
            <span>Shipping</span>
            <span>$5.00</span>
          </div>
          <div className="fk-price-row total">
            <span>Total</span>
            <span>${(cart.total + 5).toFixed(2)}</span>
          </div>
        </div>
        <div className="fk-savings-note" style={{ fontSize: 13 }}>
          Includes $5.00 shipping fee
        </div>
      </div>
    </div>
  );
}
