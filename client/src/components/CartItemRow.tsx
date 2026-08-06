import type { CartItem } from "../types";

interface Props {
  item: CartItem;
  onUpdateQuantity: (itemId: string, quantity: number) => void;
  onRemove: (itemId: string) => void;
}

export function CartItemRow({ item, onUpdateQuantity, onRemove }: Props) {
  return (
    <div className="fk-cart-item">
      {/* Placeholder image */}
      <div className="fk-cart-item-img-placeholder">
        📦
      </div>

      <div className="fk-cart-item-details">
        <p className="fk-cart-item-name">{item.productName}</p>
        <p className="fk-cart-item-unit-price">${item.unitPrice.toFixed(2)} each</p>
        <div className="fk-cart-item-controls">
          <button
            className="fk-qty-btn"
            onClick={() => onUpdateQuantity(item.id, item.quantity - 1)}
            disabled={item.quantity <= 1}
            aria-label="Decrease quantity"
          >
            &minus;
          </button>
          <span className="fk-qty-display">{item.quantity}</span>
          <button
            className="fk-qty-btn"
            onClick={() => onUpdateQuantity(item.id, item.quantity + 1)}
            aria-label="Increase quantity"
          >
            +
          </button>
        </div>
        <button className="fk-cart-item-remove" onClick={() => onRemove(item.id)}>
          REMOVE
        </button>
      </div>

      <div className="fk-cart-item-line-total">
        ${item.lineTotal.toFixed(2)}
      </div>
    </div>
  );
}
