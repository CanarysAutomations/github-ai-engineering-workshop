import { useEffect, useState } from "react";
import { useLocation, useParams, useNavigate, Link } from "react-router-dom";
import { ordersApi } from "../api/ordersApi";
import type { Order } from "../types";

export function OrderConfirmationPage() {
  const location = useLocation();
  const { orderId } = useParams<{ orderId: string }>();
  const navigate = useNavigate();
  const stateOrder = (location.state as { order?: Order } | null)?.order;
  const [order, setOrder] = useState<Order | null>(stateOrder ?? null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (order || !orderId) return;
    ordersApi.getById(orderId).then(setOrder).catch(() => setError("Order not found."));
  }, [order, orderId]);

  if (error) {
    return (
      <div className="fk-confirmation-wrap">
        <div className="fk-error-banner">{error}</div>
        <button className="fk-btn-continue" onClick={() => navigate("/")}>Back to Catalog</button>
      </div>
    );
  }

  if (!order) {
    return <div className="fk-loading-page">Loading order…</div>;
  }

  return (
    <div className="fk-confirmation-wrap">
      {/* Success banner */}
      <div className="fk-confirmation-banner">
        <svg width="36" height="36" viewBox="0 0 24 24" fill="currentColor">
          <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z" />
        </svg>
        <div>
          <h1>Order Placed Successfully!</h1>
          <p>Thank you! Your order has been confirmed and is being processed.</p>
        </div>
      </div>

      {/* Order card */}
      <div className="fk-order-card">
        <div className="fk-order-card-header">
          <div>
            <h2>Order Details</h2>
            <p className="fk-order-id-label">Order ID: {order.id}</p>
          </div>
          <span className="fk-order-status-badge">{order.status}</span>
        </div>

        <table className="fk-order-items-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Unit Price</th>
              <th>Qty</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            {order.items.map((item) => (
              <tr key={item.id}>
                <td>{item.productName}</td>
                <td>${item.unitPrice.toFixed(2)}</td>
                <td>{item.quantity}</td>
                <td>${item.lineTotal.toFixed(2)}</td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="fk-order-total-section">
          <div className="fk-order-total-row">
            <span>Subtotal</span>
            <span>${order.subtotal.toFixed(2)}</span>
          </div>
          <div className="fk-order-total-row">
            <span>Shipping</span>
            <span>${order.shippingCost.toFixed(2)}</span>
          </div>
          <div className="fk-order-total-row grand">
            <span>Grand Total</span>
            <span>${order.totalAmount.toFixed(2)}</span>
          </div>
        </div>

        <div className="fk-shipping-address-card">
          <h3>Delivering to</h3>
          <p>
            <strong>{order.shippingAddress.name}</strong><br />
            {order.shippingAddress.address}<br />
            {order.shippingAddress.city} — {order.shippingAddress.zip}
          </p>
        </div>

        <div className="fk-order-actions">
          <Link to="/" className="fk-btn-outline">Continue Shopping</Link>
          <Link to="/orders" className="fk-btn-solid">My Orders</Link>
        </div>
      </div>
    </div>
  );
}
