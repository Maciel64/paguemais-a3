import { api } from "@/lib/api";
import { CreateUserSchema, User } from "@/types/user";

export const clientsService = {
  getAll: async (): Promise<User[]> => {
    const req = await api.get("/clients");

    return req.data;
  },

  getById: async (id: string): Promise<User> => {
    const req = await api.get(`/clients/${id}`);

    return req.data;
  },

  create: async (data: CreateUserSchema): Promise<User> => {
    const { birthDate } = data;

    data.birthDate = new Date(birthDate).toISOString();

    const req = await api.post("/clients", data);

    return req.data;
  },

  update: async (id: string, data: CreateUserSchema): Promise<User> => {
    const req = await api.put(`/clients/${id}`, data);

    return req.data;
  },

  delete: async (id: string) => {
    const req = await api.delete(`/clients/${id}`);
  },
};
