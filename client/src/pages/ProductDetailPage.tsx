import { useEffect, useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { catalogApi } from "../api/catalogApi";
import { cartApi } from "../api/cartApi";
import { useCart } from "../context/CartContext";
import type { Product } from "../types";

function fakeDiscount(price: number): number {
  return 10 + (Math.round(price * 7) % 31);
}

export function ProductDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { guestId, refreshCart } = useCart();
  const [product, setProduct] = useState<Product | null>(null);
  const [quantity, setQuantity] = useState(1);
  const [error, setError] = useState<string | null>(null);
  const [adding, setAdding] = useState(false);
  const [added, setAdded] = useState(false);

  useEffect(() => {
    if (!id) return;
    catalogApi.getById(id).then(setProduct).catch(() => setError("Product not found."));
  }, [id]);

  async function handleAddToCart() {
    if (!product) return;
    setAdding(true);
    setError(null);
    setAdded(false);
    try {
      await cartApi.addItem(guestId, product.id, quantity);
      await refreshCart();
      setAdded(true);
    } catch {
      setError("Could not add item to cart. Check stock availability.");
    } finally {
      setAdding(false);
    }
  }

  async function handleBuyNow() {
    await handleAddToCart();
    navigate("/checkout");
  }

  if (error && !product) {
    return (
      <div className="fk-detail-wrap">
        <div className="fk-error-banner">{error}</div>
        <button className="fk-btn-continue" onClick={() => navigate("/")}>Back to Catalog</button>
      </div>
    );
  }

  if (!product) {
    return <div className="fk-loading-page">Loading product…</div>;
  }

  const discount = fakeDiscount(product.price);
  const originalPrice = product.price / (1 - discount / 100);

  return (
    <div className="fk-detail-wrap">
      {/* Breadcrumb */}
      <div className="fk-breadcrumb">
        <Link to="/">Home</Link>
        <span>/</span>
        <Link to="/">{product.category}</Link>
        <span>/</span>
        <span>{product.name}</span>
      </div>

      <div className="fk-detail-card">
        {/* Image + action buttons */}
        <div className="fk-detail-image-section">
          <img
            src={product.imageUrl}
            alt={product.name}
            onError={(e) => { (e.target as HTMLImageElement).src = "https://placehold.co/280x280?text=No+Image"; }}
          />
          {product.inStock && (
            <div className="fk-detail-action-btns">
              <button className="fk-btn-cart" onClick={handleAddToCart} disabled={adding}>
                <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
                  <path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96C5 16.1 6.1 17 7 17h11v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63H15c.75 0 1.41-.41 1.75-1.03l3.58-6.49A1 1 0 0 0 19.5 4H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z" />
                </svg>
                {adding ? "Adding…" : "ADD TO CART"}
              </button>
              <button className="fk-btn-buy" onClick={handleBuyNow} disabled={adding}>
                <svg width="18" height="18" viewBox="0 0 24 24" fill="currentColor">
                  <path d="M19 7h-3V6a4 4 0 0 0-8 0v1H5a1 1 0 0 0-1 1v11a3 3 0 0 0 3 3h10a3 3 0 0 0 3-3V8a1 1 0 0 0-1-1zm-9-1a2 2 0 0 1 4 0v1h-4V6zm8 13a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V9h2v1a1 1 0 0 0 2 0V9h4v1a1 1 0 0 0 2 0V9h2v10z" />
                </svg>
                BUY NOW
              </button>
            </div>
          )}
          {!product.inStock && (
            <p className="fk-detail-stock fk-out-of-stock-text">❌ Out of Stock</p>
          )}
        </div>

        {/* Product info */}
        <div className="fk-detail-info">
          <p className="fk-detail-category">{product.category}</p>
          <h1 className="fk-detail-name">{product.name}</h1>

          <div className="fk-detail-price-section">
            <p className="fk-special-price-label">Special Price</p>
            <div className="fk-detail-price-row">
              <span className="fk-detail-price">${product.price.toFixed(2)}</span>
              <span className="fk-detail-original-price">${originalPrice.toFixed(2)}</span>
              <span className="fk-detail-discount">{discount}% off</span>
            </div>
          </div>

          {product.inStock ? (
            <p className="fk-detail-stock">✅ In stock ({product.stockQuantity} available)</p>
          ) : (
            <p className="fk-detail-stock fk-out-of-stock-text">❌ Out of Stock</p>
          )}

          {product.inStock && (
            <div className="fk-qty-row">
              <label htmlFor="qty">Quantity</label>
              <select
                id="qty"
                className="fk-qty-select"
                value={quantity}
                onChange={(e) => setQuantity(Number(e.target.value))}
              >
                {Array.from({ length: Math.min(product.stockQuantity, 10) }, (_, i) => i + 1).map((n) => (
                  <option key={n} value={n}>{n}</option>
                ))}
              </select>
            </div>
          )}

          <hr className="fk-detail-separator" />

          <p className="fk-detail-description-title">Description</p>
          <p className="fk-detail-description">{product.description}</p>

          {added && <div className="fk-success-toast">✔ Added to cart successfully!</div>}
          {error && <div className="fk-error-msg">{error}</div>}
        </div>
      </div>
    </div>
  );
}
