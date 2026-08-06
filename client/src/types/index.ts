export interface Product {
  id: string;
  name: string;
  description: string;
  category: string;
  price: number;
  sku: string;
  stockQuantity: number;
  imageUrl: string;
  isActive: boolean;
  inStock: boolean;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export interface CartItem {
  id: string;
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export interface CartData {
  id: string;
  guestId: string;
  items: CartItem[];
  total: number;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
  userId: string;
  username: string;
}

export interface ShippingAddress {
  name: string;
  address: string;
  city: string;
  zip: string;
}

export interface OrderItem {
  id: string;
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export interface Order {
  id: string;
  userId: string;
  status: string;
  shippingAddress: ShippingAddress;
  items: OrderItem[];
  subtotal: number;
  shippingCost: number;
  totalAmount: number;
  createdAt: string;
}

export interface ErrorResponse {
  message: string;
}
