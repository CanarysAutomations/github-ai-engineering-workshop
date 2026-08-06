import { useState, type FormEvent } from "react";
import type { ShippingAddress } from "../types";

export function ShippingAddressForm({ onSubmit, submitting }: { onSubmit: (address: ShippingAddress) => void; submitting: boolean }) {
  const [address, setAddress] = useState<ShippingAddress>({ name: "", address: "", city: "", zip: "" });

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    onSubmit(address);
  }

  return (
    <div className="fk-checkout-form-card">
      <h2>Delivery Address</h2>
      <form onSubmit={handleSubmit}>
        <div className="fk-form-group">
          <label htmlFor="ship-name">Full Name</label>
          <input
            id="ship-name"
            className="fk-form-input"
            placeholder="Enter full name"
            value={address.name}
            onChange={(e) => setAddress({ ...address, name: e.target.value })}
            required
          />
        </div>
        <div className="fk-form-group">
          <label htmlFor="ship-address">Address (Area and Street)</label>
          <input
            id="ship-address"
            className="fk-form-input"
            placeholder="House No, Building, Street, Area"
            value={address.address}
            onChange={(e) => setAddress({ ...address, address: e.target.value })}
            required
          />
        </div>
        <div className="fk-form-group">
          <label htmlFor="ship-city">City / District / Town</label>
          <input
            id="ship-city"
            className="fk-form-input"
            placeholder="Enter city"
            value={address.city}
            onChange={(e) => setAddress({ ...address, city: e.target.value })}
            required
          />
        </div>
        <div className="fk-form-group">
          <label htmlFor="ship-zip">Pincode</label>
          <input
            id="ship-zip"
            className="fk-form-input"
            placeholder="6-digit pincode"
            value={address.zip}
            onChange={(e) => setAddress({ ...address, zip: e.target.value })}
            required
          />
        </div>
        <button type="submit" className="fk-btn-submit" disabled={submitting}>
          {submitting ? "Processing payment…" : "Confirm Order &amp; Pay"}
        </button>
      </form>
    </div>
  );
}
