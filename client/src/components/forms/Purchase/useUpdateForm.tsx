import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "../../ui/use-toast";
import { purchaseService } from "@/services/purchase-service";
import { AxiosError } from "axios";
import { useClient } from "@/app/clients/useClient";
import { useClients } from "@/app/clients/useClients";
import { CreatePurchaseSchema, UpdatePurchaseSchema } from "@/types/purchase";
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

  const { client, setClient } = useClient();
  const { closeDialog } = useClients();

  const queryClient = useQueryClient();

  const updateMutation = useMutation({
    mutationFn: purchaseService.update,
    onSuccess: () => {
      toast({
        title: `Cliente ${client?.name} atualizado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["purchases"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível atualizar a compra ${client?.name}`,
        description: error.request.response,
      });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: purchaseService.delete,
    onSuccess: () => {
      toast({
        title: `Cliente ${client?.name} deletado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["purchases"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível apagar o cliente ${client?.name}`,
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
