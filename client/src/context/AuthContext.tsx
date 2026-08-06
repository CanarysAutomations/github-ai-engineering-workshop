import { createContext, useContext, useState, type ReactNode } from "react";
import { identityApi } from "../api/identityApi";
import { setAuthToken } from "../api/httpClient";

interface AuthContextValue {
  token: string | null;
  username: string | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(null);
  const [username, setUsername] = useState<string | null>(null);

  async function login(usernameInput: string, password: string) {
    const response = await identityApi.login(usernameInput, password);
    setToken(response.token);
    setUsername(response.username);
    setAuthToken(response.token);
  }

  function logout() {
    setToken(null);
    setUsername(null);
    setAuthToken(null);
  }

  return (
    <AuthContext.Provider value={{ token, username, isAuthenticated: !!token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
