import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ordersApi } from "../api/ordersApi";
import { useAuth } from "../context/AuthContext";
import type { Order } from "../types";

export function OrderHistoryPage() {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!isAuthenticated) {
      setLoading(false);
      return;
    }
    ordersApi
      .list()
      .then(setOrders)
      .catch(() => setError("Unable to load order history."))
      .finally(() => setLoading(false));
  }, [isAuthenticated]);

  if (!isAuthenticated) {
    return (
      <div className="fk-not-auth">
        <h1>My Orders</h1>
        <p>Please log in to view your order history.</p>
        <button
          className="fk-btn-continue"
          onClick={() => navigate("/login", { state: { from: "/orders" } })}
        >
          Login
        </button>
      </div>
    );
  }

  if (loading) return <div className="fk-loading-page">Loading orders…</div>;
  if (error)   return <div className="fk-orders-wrap"><div className="fk-error-banner">{error}</div></div>;

  return (
    <div className="fk-orders-wrap">
      <h1>My Orders</h1>
      {orders.length === 0 ? (
        <div className="fk-empty">You haven’t placed any orders yet.</div>
      ) : (
        orders.map((order) => (
          <Link
            key={order.id}
            to={`/order-confirmation/${order.id}`}
            className="fk-order-history-item"
          >
            <div>
              <p className="fk-order-history-id">Order ID: {order.id.slice(0, 8)}&hellip;</p>
              <p className="fk-order-history-total">${order.totalAmount.toFixed(2)}</p>
              <p className="fk-order-history-date">
                {new Date(order.createdAt).toLocaleString()}
              </p>
            </div>
            <span className="fk-order-history-status">{order.status}</span>
          </Link>
        ))
      )}
    </div>
  );
}
