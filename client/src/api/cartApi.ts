import httpClient from "./httpClient";
import type { CartData } from "../types";

export const cartApi = {
  async get(guestId: string) {
    const response = await httpClient.get<CartData>(`/api/cart/${guestId}`);
    return response.data;
  },

  async addItem(guestId: string, productId: string, quantity: number) {
    const response = await httpClient.post<CartData>(`/api/cart/${guestId}/items`, { productId, quantity });
    return response.data;
  },

  async updateItem(guestId: string, itemId: string, quantity: number) {
    const response = await httpClient.put<CartData>(`/api/cart/${guestId}/items/${itemId}`, { quantity });
    return response.data;
  },

  async removeItem(guestId: string, itemId: string) {
    const response = await httpClient.delete<CartData>(`/api/cart/${guestId}/items/${itemId}`);
    return response.data;
  },
};
