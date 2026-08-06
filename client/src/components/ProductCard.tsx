import { Link } from "react-router-dom";
import type { Product } from "../types";

/** Deterministic fake discount based on price so cards look consistent */
function fakeDiscount(price: number): number {
  return 10 + (Math.round(price * 7) % 31); // 10-40%
}

/** Deterministic rating 3.8 - 4.8 */
function fakeRating(name: string): number {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return +(3.8 + (code % 10) / 10).toFixed(1);
}

export function ProductCard({ product }: { product: Product }) {
  const discount = fakeDiscount(product.price);
  const originalPrice = product.price / (1 - discount / 100);
  const rating = fakeRating(product.name);

  return (
    <Link to={`/products/${product.id}`} className="fk-product-card">
      <div className="fk-product-img-wrap">
        <img
          src={product.imageUrl}
          alt={product.name}
          className="fk-product-img"
          onError={(e) => { (e.target as HTMLImageElement).src = "https://placehold.co/160x160?text=No+Image"; }}
        />
        {!product.inStock && (
          <span className="fk-out-of-stock-badge">Out of Stock</span>
        )}
      </div>
      <div className="fk-product-info">
        <p className="fk-product-name">{product.name}</p>
        <div className="fk-product-meta">
          <span className="fk-rating">{rating} ★</span>
          <span className="fk-rating-count">(1,{Math.floor(100 + (product.price % 900))})</span>
        </div>
        <div className="fk-product-pricing">
          <span className="fk-price">${product.price.toFixed(2)}</span>
          <span className="fk-original-price">${originalPrice.toFixed(2)}</span>
          <span className="fk-discount">{discount}% off</span>
        </div>
        <p className="fk-free-delivery">Free delivery</p>
      </div>
    </Link>
  );
}
