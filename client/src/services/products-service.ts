import { api } from "@/lib/api";
import {
  CreateProductSchema,
  Product,
  UpdateProductSchema,
} from "@/types/product";

export const productsService = {
  getAll: async (): Promise<Product[]> => {
    const req = await api.get("/products");

    return req.data;
  },

  getById: async (id: string): Promise<Product> => {
    const req = await api.get(`/products/${id}`);

    return req.data;
  },

  create: async ({ data }: { data: CreateProductSchema }): Promise<Product> => {
    const req = await api.post("/products", data);
    return req.data;
  },

  update: async ({
    id,
    data,
  }: {
    id: string;
    data: UpdateProductSchema;
  }): Promise<Product> => {
    const req = await api.put(`/products/${id}`, data);
    return req.data;
  },

  delete: async (id: string) => {
    const req = await api.delete(`/products/${id}`);
  },
};
