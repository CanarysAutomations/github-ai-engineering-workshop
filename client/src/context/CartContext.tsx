import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from "react";
import { cartApi } from "../api/cartApi";
import { getOrCreateGuestId } from "../utils/guestId";
import type { CartData } from "../types";

interface CartContextValue {
  guestId: string;
  cart: CartData | null;
  itemCount: number;
  refreshCart: () => Promise<void>;
  clearLocalCart: () => void;
}

const CartContext = createContext<CartContextValue | undefined>(undefined);

export function CartProvider({ children }: { children: ReactNode }) {
  const [guestId] = useState<string>(() => getOrCreateGuestId());
  const [cart, setCart] = useState<CartData | null>(null);

  const refreshCart = useCallback(async () => {
    const data = await cartApi.get(guestId);
    setCart(data);
  }, [guestId]);

  function clearLocalCart() {
    setCart((prev) => (prev ? { ...prev, items: [], total: 0 } : prev));
  }

  useEffect(() => {
    refreshCart().catch(() => {
      /* ignore initial load failure, e.g. backend not up yet */
    });
  }, [refreshCart]);

  const itemCount = cart?.items.reduce((sum, item) => sum + item.quantity, 0) ?? 0;

  return (
    <CartContext.Provider value={{ guestId, cart, itemCount, refreshCart, clearLocalCart }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart must be used within a CartProvider");
  }
  return context;
}
