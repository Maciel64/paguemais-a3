import { productsService } from "@/services/products-service";
import { Product, UseProductState } from "@/types/product";
import { UseUIState } from "@/types/ui";
import { useQuery } from "@tanstack/react-query";
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { useMemo } from "react";
import { create } from "zustand";

export const useUI = create<UseUIState & UseProductState>((set) => ({
  dialogUIState: "closed",
  setDialogUIState: (dialogUIState) => set({ dialogUIState }),
  product: null,
  setProduct: (product) => set({ product }),
  closeDialog: () => set({ product: null, dialogUIState: "closed" }),
  setIsDeleting: (product) => set({ product, dialogUIState: "deleting" }),
  setIsUpdating: (product) => set({ product, dialogUIState: "updating" }),
}));

export const useProducts = () => {
  const {
    dialogUIState,
    setDialogUIState,
    product,
    closeDialog,
    setIsDeleting,
    setIsUpdating,
  } = useUI();

  const columns: ColumnDef<Product>[] = useMemo(
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
        accessorKey: "price",
        header: "Preço",
      },
      {
        accessorKey: "createdAt",
        header: "Cadastrado Em",
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
    queryKey: ["products"],
    queryFn: productsService.getAll,
  });

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return {
    columns,
    table,
    isLoading,
    product,
    products: data,
    closeDialog,
    dialogUIState,
    setDialogUIState,
    setIsDeleting,
    setIsUpdating,
  };
};
