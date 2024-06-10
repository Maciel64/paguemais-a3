import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "../ui/use-toast";
import { purchaseService } from "@/services/purchase-service";
import { AxiosError } from "axios";
import { useClient } from "@/app/clients/useClient";
import { useClients } from "@/app/clients/useClients";
import { CreatePurchaseSchema, UpdatePurchaseSchema } from "@/types/purchase";
import { clientsService } from "@/services/clients-service";

const createPurchaseSchema = z.object({
  clientId: z.string(),
  paymentMethod: z.string(),
});

type typeCreatePurchaseSchema = z.infer<typeof createPurchaseSchema>;

export const usePurchaseForm = () => {
  const {
    handleSubmit,
    register,
    control,
    formState: { errors },
  } = useForm<typeCreatePurchaseSchema>({
    resolver: zodResolver(createPurchaseSchema),
  });

  console.log(errors);

  const { client, setClient } = useClient();
  const { closeDialog } = useClients();

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: purchaseService.create,
    onSuccess: () => {
      toast({
        title: "Sucesso!",
      });
      queryClient.invalidateQueries({ queryKey: ["clients"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível criar um novo usuário",
        description: error.request.response,
      });
    },
  });

  const updateMutation = useMutation({
    mutationFn: purchaseService.update,
    onSuccess: () => {
      toast({
        title: `Cliente ${client?.name} atualizado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["clients"] });
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
      queryClient.invalidateQueries({ queryKey: ["clients"] });
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
    createMutation.isPending ||
    updateMutation.isPending ||
    deleteMutation.isPending;

  const create: SubmitHandler<CreatePurchaseSchema> = (data) =>
    createMutation.mutate({ data });

  const update = (id: string, data: UpdatePurchaseSchema) =>
    updateMutation.mutate({ id, data });

  return {
    handleSubmit,
    register,
    control,
    errors,
    create,
    update,
    isMutationLoading,
    deleteMutation,
    clients,
    isClientsLoading,
  };
};
