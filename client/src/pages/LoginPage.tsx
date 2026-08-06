import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { LoginForm } from "../components/LoginForm";

export function LoginPage() {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Where to go after login — default to home
  const from = (location.state as { from?: string } | null)?.from ?? "/";

  // If already logged in, redirect immediately
  useEffect(() => {
    if (isAuthenticated) navigate(from, { replace: true });
  }, [isAuthenticated, from, navigate]);

  return (
    <div className="fk-login-page-wrap">
      <LoginForm onSuccess={() => navigate(from, { replace: true })} />
    </div>
  );
}
