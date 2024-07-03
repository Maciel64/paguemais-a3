import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "../../ui/use-toast";
import { purchaseService } from "@/services/purchase-service";
import { AxiosError } from "axios";
import { useClients } from "@/app/clients/useClients";
import { UpdatePurchaseSchema } from "@/types/purchase";
import { clientsService } from "@/services/clients-service";

const updatingPurchaseSchema = z.object({
  status: z.string(),
  paymentMethod: z.string(),
});

export const useUpdateForm = () => {
  const {
    handleSubmit,
    register,
    control,
    reset,
    formState: { errors },
  } = useForm<z.infer<typeof updatingPurchaseSchema>>({
    resolver: zodResolver(updatingPurchaseSchema),
  });

  const { closeDialog } = useClients();

  const queryClient = useQueryClient();

  const updateMutation = useMutation({
    mutationFn: purchaseService.update,
    onSuccess: () => {
      toast({
        title: `Compra atualizada com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["purchases"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível atualizar a compra`,
        description: error.request.response,
      });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: purchaseService.delete,
    onSuccess: () => {
      toast({
        title: `Compra deletada com sucesso!`,
      });
      closeDialog();
      setTimeout(() => {
        queryClient.invalidateQueries({ queryKey: ["purchases"] });
      }, 300);
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível apagar a compra`,
        description: error.request.response,
      });
    },
  });

  const {
    data: clients = [],
    error,
    isLoading: isClientsLoading,
  } = useQuery({
    queryKey: ["clients"],
    queryFn: clientsService.getAll,
  });

  const isMutationLoading =
    updateMutation.isPending || deleteMutation.isPending;

  const update = (id: string, data: UpdatePurchaseSchema) =>
    updateMutation.mutate({ id, data });

  return {
    handleSubmit,
    register,
    control,
    errors,
    update,
    reset,
    isMutationLoading,
    deleteMutation,
    clients,
    isClientsLoading,
  };
};
