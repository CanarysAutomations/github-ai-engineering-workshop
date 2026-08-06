import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { catalogApi } from "../api/catalogApi";
import { ProductCard } from "../components/ProductCard";
import type { Product } from "../types";

const CATEGORIES = ["Electronics", "Apparel", "Home"];

export function CatalogPage() {
  const [searchParams] = useSearchParams();
  const [products, setProducts] = useState<Product[]>([]);
  const [category, setCategory] = useState("");
  const [search, setSearch] = useState(searchParams.get("search") ?? "");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setSearch(searchParams.get("search") ?? "");
  }, [searchParams]);

  useEffect(() => {
    setLoading(true);
    setError(null);
    catalogApi
      .list({ category: category || undefined, search: search || undefined, page: 1, pageSize: 50 })
      .then((result) => setProducts(result.items))
      .catch(() => setError("Unable to load catalog. Is the backend running?"))
      .finally(() => setLoading(false));
  }, [category, search]);

  return (
    <div className="fk-catalog-layout">
      {/* Sidebar */}
      <aside className="fk-sidebar">
        <p className="fk-sidebar-title">Filters</p>
        <div className="fk-sidebar-section">
          <h4>Category</h4>
          <label className="fk-radio-label">
            <input
              type="radio"
              name="category"
              checked={category === ""}
              onChange={() => setCategory("")}
            />
            All
          </label>
          {CATEGORIES.map((c) => (
            <label key={c} className="fk-radio-label">
              <input
                type="radio"
                name="category"
                checked={category === c}
                onChange={() => setCategory(c)}
              />
              {c}
            </label>
          ))}
        </div>
      </aside>

      {/* Main content */}
      <div className="fk-catalog-main">
        <div className="fk-catalog-toolbar">
          {search
            ? `${products.length} results for "${search}"`
            : category
            ? `${products.length} results in ${category}`
            : `All products (${products.length})`}
        </div>

        {loading && (
          <div className="fk-loading">
            {Array.from({ length: 8 }).map((_, i) => (
              <div key={i} className="fk-skeleton" />
            ))}
          </div>
        )}

        {error && <div className="fk-error-banner">{error}</div>}

        {!loading && !error && products.length === 0 && (
          <div className="fk-empty">No products found.</div>
        )}

        {!loading && !error && products.length > 0 && (
          <div className="fk-product-grid">
            {products.map((p) => (
              <ProductCard key={p.id} product={p} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
