import httpClient from "./httpClient";
import type { PagedResult, Product } from "../types";

export const catalogApi = {
  async list(params: { category?: string; search?: string; page?: number; pageSize?: number }) {
    const response = await httpClient.get<PagedResult<Product>>("/api/catalog/products", { params });
    return response.data;
  },

  async getById(id: string) {
    const response = await httpClient.get<Product>(`/api/catalog/products/${id}`);
    return response.data;
  },
};
