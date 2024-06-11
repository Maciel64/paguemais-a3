import { useUpdateForm } from "@/components/forms/Purchase/useUpdateForm";
import { purchaseService } from "@/services/purchase-service";
import {
  Purchase,
  UsePurchaseState,
  UsePurchaseUIState,
} from "@/types/purchase";
import { UseUIState } from "@/types/ui";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { useMemo } from "react";
import { create } from "zustand";
import { useProducts } from "./products/useProducts";
import { toast } from "@/components/ui/use-toast";
import { AxiosError } from "axios";

export const useUI = create<UsePurchaseUIState & UsePurchaseState>((set) => ({
  dialogUIState: "closed",
  setDialogUIState: (dialogUIState) => set({ dialogUIState }),
  purchase: null,
  product: null,
  clientId: null,
  setPurchase: (purchase) => set({ purchase }),
  setProduct: (product) => set({ product }),
  setClientId: (clientId) => set({ clientId }),
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
    setPurchase,
    clientId,
    setClientId,
    product,
    closeDialog,
    setIsDeleting,
    setIsUpdating,
    setIsAddingProducts,
  } = useUI();

  const { deleteMutation } = useUpdateForm();
  const { products } = useProducts();

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

  const queryClient = useQueryClient();

  const {
    data = [],
    error,
    isLoading,
  } = useQuery({
    queryKey: ["purchases"],
    queryFn: purchaseService.getAll,
  });

  const { data: carts = [], isLoading: isCartsLoading } = useQuery({
    queryKey: ["carts"],
    queryFn: purchaseService.getAllCarts,
  });

  const createCartMutation = useMutation({
    mutationFn: purchaseService.createCart,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["carts"] });
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível adicionar a quantidade",
        description: error.request.response,
      });
    },
  });

  const increaseQuantityMutation = useMutation({
    mutationFn: purchaseService.increaseQuantity,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["carts"] });
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível adicionar a quantidade",
        description: error.request.response,
      });
    },
  });

  const decreaseQuantityMutation = useMutation({
    mutationFn: purchaseService.decreaseQuantity,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["carts"] });
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível adicionar a quantidade",
        description: error.request.response,
      });
    },
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
    setPurchase,
    clientId,
    setClientId,
    product,
    products,
    carts,
    closeDialog,
    dialogUIState,
    setDialogUIState,
    setIsDeleting,
    setIsUpdating,
    deleteMutation,
    paymentMethodMapper,
    createCartMutation,
    increaseQuantityMutation,
    decreaseQuantityMutation,
  };
};
