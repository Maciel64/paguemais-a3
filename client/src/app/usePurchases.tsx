import { useUpdateForm } from "@/components/forms/Purchase/useUpdateForm";
import { purchaseService } from "@/services/purchase-service";
import {
  Purchase,
  UsePurchaseState,
  UsePurchaseUIState,
} from "@/types/purchase";
import { UseUIState } from "@/types/ui";
import { useQuery } from "@tanstack/react-query";
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { useMemo } from "react";
import { create } from "zustand";

export const useUI = create<UsePurchaseUIState & UsePurchaseState>((set) => ({
  dialogUIState: "closed",
  setDialogUIState: (dialogUIState) => set({ dialogUIState }),
  purchase: null,
  product: null,
  client: null,
  setPurchase: (purchase) => set({ purchase }),
  setProduct: (product) => set({ product }),
  setClient: (client) => set({ client }),
  closeDialog: () => set({ purchase: null, dialogUIState: "closed" }),
  setIsDeleting: (purchase) => set({ purchase, dialogUIState: "deleting" }),
  setIsUpdating: (purchase) => set({ purchase, dialogUIState: "updating" }),
  setIsAddingProducts: () => set({ dialogUIState: "addingProducts" }),
}));

const paymentMethodMapper = ["Crédito", "Débito", "Pix", "Dinheiro"];

export const usePurchases = () => {
  const {
    dialogUIState,
    setDialogUIState,
    purchase,
    client,
    product,
    closeDialog,
    setIsDeleting,
    setIsUpdating,
    setIsAddingProducts,
  } = useUI();

  const { deleteMutation } = useUpdateForm();

  const columns: ColumnDef<Purchase>[] = useMemo(
    () => [
      {
        accessorKey: "id",
        header: "Id",
      },
      {
        accessorKey: "clientId",
        header: "Id do client",
      },
      {
        accessorKey: "paymentMethod",
        header: "Pagamento",
      },
      {
        accessorKey: "total",
        header: "Total",
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
      {
        accessorKey: "products",
        header: "Produtos",
      },
    ],
    []
  );

  const {
    data = [],
    error,
    isLoading,
  } = useQuery({
    queryKey: ["purchases"],
    queryFn: purchaseService.getAll,
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
    purchase,
    client,
    product,
    closeDialog,
    dialogUIState,
    setDialogUIState,
    setIsDeleting,
    setIsUpdating,
    deleteMutation,
    paymentMethodMapper,
  };
};
