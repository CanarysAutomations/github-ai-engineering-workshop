import { useNavigate } from "react-router-dom";
import { cartApi } from "../api/cartApi";
import { useCart } from "../context/CartContext";
import { CartItemRow } from "../components/CartItemRow";

export function CartPage() {
  const { guestId, cart, refreshCart } = useCart();
  const navigate = useNavigate();

  async function handleUpdateQuantity(itemId: string, quantity: number) {
    if (quantity < 1) return;
    await cartApi.updateItem(guestId, itemId, quantity);
    await refreshCart();
  }

  async function handleRemove(itemId: string) {
    await cartApi.removeItem(guestId, itemId);
    await refreshCart();
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="fk-empty-cart">
        <span className="fk-empty-cart-icon">🛒</span>
        <h2>Your cart is empty!</h2>
        <p>Add items to it now.</p>
        <button className="fk-btn-continue" onClick={() => navigate("/")}>
          Shop Now
        </button>
      </div>
    );
  }

  const savings = cart.items.reduce((acc, item) => {
    const disc = 10 + (Math.round(item.unitPrice * 7) % 31);
    const orig = item.unitPrice / (1 - disc / 100);
    return acc + (orig - item.unitPrice) * item.quantity;
  }, 0);

  return (
    <div className="fk-cart-layout">
      {/* Cart items */}
      <div className="fk-cart-main">
        <div className="fk-cart-header">
          <h1>My Cart</h1>
          <span>{cart.items.length} item{cart.items.length !== 1 ? "s" : ""}</span>
        </div>
        <div className="fk-cart-items">
          {cart.items.map((item) => (
            <CartItemRow
              key={item.id}
              item={item}
              onUpdateQuantity={handleUpdateQuantity}
              onRemove={handleRemove}
            />
          ))}
        </div>
        <div className="fk-cart-footer">
          <button className="fk-btn-place-order" onClick={() => navigate("/checkout")}>
            PLACE ORDER
          </button>
        </div>
      </div>

      {/* Price summary */}
      <div className="fk-price-summary">
        <div className="fk-price-summary-header">Price Details</div>
        <div className="fk-price-summary-rows">
          <div className="fk-price-row">
            <span>Price ({cart.items.length} item{cart.items.length !== 1 ? "s" : ""})</span>
            <span>${(cart.total + savings).toFixed(2)}</span>
          </div>
          <div className="fk-price-row discount">
            <span>Discount</span>
            <span>−${savings.toFixed(2)}</span>
          </div>
          <div className="fk-price-row">
            <span>Delivery Charges</span>
            <span style={{ color: '#388e3c' }}>FREE</span>
          </div>
          <div className="fk-price-row total">
            <span>Total Amount</span>
            <span>${cart.total.toFixed(2)}</span>
          </div>
        </div>
        <div className="fk-savings-note">
          You will save ${savings.toFixed(2)} on this order
        </div>
      </div>
    </div>
  );
}
