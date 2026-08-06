import httpClient from "./httpClient";
import type { Order, ShippingAddress } from "../types";

export const ordersApi = {
  async checkout(guestId: string, shippingAddress: ShippingAddress) {
    const response = await httpClient.post<Order>("/api/orders/checkout", { guestId, shippingAddress });
    return response.data;
  },

  async list() {
    const response = await httpClient.get<Order[]>("/api/orders");
    return response.data;
  },

  async getById(orderId: string) {
    const response = await httpClient.get<Order>(`/api/orders/${orderId}`);
    return response.data;
  },
};
