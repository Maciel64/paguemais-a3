import { api } from "@/lib/api";

export const clientsService = {
  getAll: async () => {
    const req = await api.get("/clients");

    return req.data;
  },
};
