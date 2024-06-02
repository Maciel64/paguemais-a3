import { api } from "@/lib/api";
import { CreateClientSchema, Client, UpdateClientSchema } from "@/types/client";

export const clientsService = {
  getAll: async (): Promise<Client[]> => {
    const req = await api.get("/clients");

    return req.data;
  },

  getById: async (id: string): Promise<Client> => {
    const req = await api.get(`/clients/${id}`);

    return req.data;
  },

  create: async ({ data }: { data: CreateClientSchema }): Promise<Client> => {
    const { birthDate } = data;

    data.birthDate = new Date(birthDate).toISOString();

    const req = await api.post("/clients", data);

    return req.data;
  },

  update: async ({
    id,
    data,
  }: {
    id: string;
    data: UpdateClientSchema;
  }): Promise<Client> => {
    const { birthDate } = data;

    if (birthDate) {
      data.birthDate = new Date(birthDate).toISOString();
    }

    const req = await api.put(`/clients/${id}`, data);

    return req.data;
  },

  delete: async (id: string) => {
    const req = await api.delete(`/clients/${id}`);
  },
};
