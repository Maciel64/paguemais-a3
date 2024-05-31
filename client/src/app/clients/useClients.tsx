import { useQuery } from "@tanstack/react-query";
import { clientsService } from "../services/clients-service";
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { Client } from "@/types/client";
import { useMemo, useState } from "react";
import { useClient } from "./useClient";
import { create } from "zustand";

type DialogUIStates = "closed" | "deleting" | "creating" | "updating";

interface UseUIState {
  dialogUIState: DialogUIStates;
  setDialogUIState: (UIState: DialogUIStates) => void;
}

export const useUI = create<UseUIState>((set) => ({
  dialogUIState: "closed",
  setDialogUIState: (dialogUIState) => set({ dialogUIState }),
}));

export const useClients = () => {
  const { client, setClient } = useClient();
  const { dialogUIState, setDialogUIState } = useUI();

  const columns: ColumnDef<Client>[] = useMemo(
    () => [
      {
        accessorKey: "id",
        header: "Id",
      },
      {
        accessorKey: "name",
        header: "Nome",
      },
      {
        accessorKey: "email",
        header: "Email",
      },
      {
        accessorKey: "cpf",
        header: "Cpf",
      },
      {
        accessorKey: "phone",
        header: "Telefone",
      },
      {
        accessorKey: "birthDate",
        header: "Data de Nascimento",
      },
      {
        accessorKey: "delete",
        header: "Apagar",
      },
      {
        accessorKey: "update",
        header: "Atualizar",
      },
    ],
    []
  );

  const {
    data = [],
    error,
    isLoading,
  } = useQuery({
    queryKey: ["clients"],
    queryFn: clientsService.getAll,
  });

  const closeDialog = () => {
    setDialogUIState("closed");
    setClient(null);
  };

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return {
    table,
    isLoading,
    columns,
    client,
    setClient,
    dialogUIState,
    setDialogUIState,
    closeDialog,
  };
};
