import { api } from "@/lib/api";
import {
  CreateCartSchema,
  CreatePurchaseSchema,
  Purchase,
  UpdatePurchaseSchema,
} from "@/types/purchase";

export const purchaseService = {
  getAll: async (): Promise<Purchase[]> => {
    const req = await api.get("/purchases");

    return req.data;
  },

  getById: async (id: string): Promise<Purchase> => {
    const req = await api.get(`/purchases/${id}`);

    return req.data;
  },

  create: async ({
    data,
  }: {
    data: CreatePurchaseSchema;
  }): Promise<Purchase> => {
    const req = await api.post("/purchases", data);
    console.log(req);
    return req.data;
  },

  update: async ({
    id,
    data,
  }: {
    id: string;
    data: UpdatePurchaseSchema;
  }): Promise<Purchase> => {
    const req = await api.put(`/purchases/${id}`, data);
    return req.data;
  },

  delete: async (id: string) => {
    api.delete(`/purchases/${id}`);
  },

  createCart: (data: CreateCartSchema) => {
    api.post(`/carts`, data);
  },

  increaseQuantity: (cartId: string) => {
    api.get(`/carts/increment/${cartId}`);
  },

  decreaseQuantity: (cartId: string) => {
    api.get(`/carts/decrement/${cartId}`);
  },
};
