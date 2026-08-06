import httpClient from "./httpClient";
import type { LoginResponse } from "../types";

export const identityApi = {
  async login(username: string, password: string) {
    const response = await httpClient.post<LoginResponse>("/api/identity/login", { username, password });
    return response.data;
  },

  async register(username: string, password: string) {
    const response = await httpClient.post<LoginResponse>("/api/identity/register", { username, password });
    return response.data;
  },
};
