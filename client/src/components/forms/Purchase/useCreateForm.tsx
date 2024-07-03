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

const createPurchaseSchema = z.object({
  clientId: z.string(),
  paymentMethod: z.string(),
});

export const useCreateForm = () => {
  const {
    handleSubmit,
    register,
    control,
    formState: { errors },
  } = useForm<z.infer<typeof createPurchaseSchema>>({
    resolver: zodResolver(createPurchaseSchema),
  });

  const { closeDialog } = useClients();

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: purchaseService.create,
    onSuccess: () => {
      toast({
        title: "Sucesso!",
      });
      queryClient.invalidateQueries({ queryKey: ["purchases"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível criar uma nova compra",
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

  const isMutationLoading = createMutation.isPending;

  const create: SubmitHandler<CreatePurchaseSchema> = (data) =>
    createMutation.mutate({ data });

  return {
    handleSubmit,
    register,
    control,
    errors,
    create,
    isMutationLoading,
    clients,
    isClientsLoading,
  };
};
