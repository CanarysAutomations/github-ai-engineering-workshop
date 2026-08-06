import { useState, type FormEvent } from "react";
import axios from "axios";
import { useAuth } from "../context/AuthContext";

export function LoginForm({ onSuccess }: { onSuccess: () => void }) {
  const { login } = useAuth();
  const [username, setUsername] = useState("demo");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);
    setSubmitting(true);
    try {
      await login(username, password);
      onSuccess();
    } catch (err) {
      if (axios.isAxiosError(err) && err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("Login failed. Please try again.");
      }
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="fk-login-card">
      {/* Left panel */}
      <div className="fk-login-left">
        <h2>Login</h2>
        <p>
          Get access to your Orders, Wishlist and Recommendations
        </p>
      </div>

      {/* Right panel — form */}
      <div className="fk-login-right">
        <div className="fk-login-hint">
          Demo account: <strong>demo</strong> / <strong>demo123</strong>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="fk-form-group">
            <label htmlFor="login-username">Username</label>
            <input
              id="login-username"
              className="fk-form-input"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              autoComplete="username"
            />
          </div>
          <div className="fk-form-group">
            <label htmlFor="login-password">Password</label>
            <input
              id="login-password"
              type="password"
              className="fk-form-input"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              autoComplete="current-password"
            />
          </div>
          {error && <div className="fk-error-msg">{error}</div>}
          <button type="submit" className="fk-btn-login" disabled={submitting}>
            {submitting ? "Logging in…" : "Login"}
          </button>
        </form>
      </div>
    </div>
  );
}
