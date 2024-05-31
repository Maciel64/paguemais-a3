import { Client } from "@/types/client";
import { create } from "zustand";

interface UseClientState {
  client: Client | null;
  setClient: (client: Client | null) => void;
}

export const useClient = create<UseClientState>((set) => ({
  client: null,
  setClient: (client) => set(() => ({ client })),
}));
