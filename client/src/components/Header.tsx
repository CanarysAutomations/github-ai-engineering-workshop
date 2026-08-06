import { useState, type FormEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useCart } from "../context/CartContext";

export function Header() {
  const { isAuthenticated, username, logout } = useAuth();
  const { itemCount } = useCart();
  const navigate = useNavigate();
  const [search, setSearch] = useState("");
  const [showMenu, setShowMenu] = useState(false);

  function handleSearch(e: FormEvent) {
    e.preventDefault();
    const q = search.trim();
    navigate(q ? `/?search=${encodeURIComponent(q)}` : "/");
  }

  return (
    <header className="fk-header">
      <div className="fk-header-inner">
        {/* Logo */}
        <Link to="/" className="fk-logo">
          <span className="fk-logo-title">eShop</span>
        </Link>

        {/* Search bar */}
        <form className="fk-search-bar" onSubmit={handleSearch}>
          <input
            type="text"
            placeholder="Search for products, brands and more"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
          <button type="submit" className="fk-search-btn" aria-label="Search">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
              <path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0 0 16 9.5 6.5 6.5 0 1 0 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z" />
            </svg>
          </button>
        </form>

        {/* Nav */}
        <nav className="fk-nav">
          {/* User menu */}
          <div
            className="fk-nav-user"
            onMouseEnter={() => setShowMenu(true)}
            onMouseLeave={() => setShowMenu(false)}
          >
            <span className="fk-nav-btn">
              {isAuthenticated ? `Hi, ${username}` : "Login"}
              <svg width="12" height="12" viewBox="0 0 24 24" fill="currentColor" style={{ marginLeft: 4 }}>
                <path d="M7 10l5 5 5-5z" />
              </svg>
            </span>
            {showMenu && (
              <div className="fk-dropdown">
                {isAuthenticated ? (
                  <>
                    <Link to="/orders">My Orders</Link>
                    <button onClick={logout}>Logout</button>
                  </>
                ) : (
                  <>
                    <div className="fk-dropdown-login">
                      <span>New customer?</span>
                      <Link to="/login">Sign Up</Link>
                    </div>
                    <Link to="/orders">My Orders</Link>
                    <Link to="/login">Login</Link>
                  </>
                )}
              </div>
            )}
          </div>

          {/* Cart */}
          <Link to="/cart" className="fk-cart-btn">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
              <path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96C5 16.1 6.1 17 7 17h11v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63H15c.75 0 1.41-.41 1.75-1.03l3.58-6.49A1 1 0 0 0 19.5 4H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z" />
            </svg>
            <span>Cart</span>
            {itemCount > 0 && <span className="fk-cart-count">{itemCount}</span>}
          </Link>
        </nav>
      </div>
    </header>
  );
}
