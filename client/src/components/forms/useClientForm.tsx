import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "../ui/use-toast";
import { clientsService } from "@/app/services/clients-service";
import { AxiosError } from "axios";
import { CreateClientSchema, UpdateClientSchema } from "@/types/client";
import { useClient } from "@/app/clients/useClient";
import { useClients } from "@/app/clients/useClients";

const createClientSchema = z.object({
  name: z.string(),
  email: z.string().email(),
  phone: z.string(),
  cpf: z.string(),
  birthDate: z.string(),
});

type typeCreateClientSchema = z.infer<typeof createClientSchema>;

export const useClientForm = () => {
  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<typeCreateClientSchema>({
    resolver: zodResolver(createClientSchema),
  });

  const { client, setClient } = useClient();
  const { closeDialog } = useClients();

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: clientsService.create,
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
    mutationFn: clientsService.update,
    onSuccess: () => {
      toast({
        title: `Cliente ${client?.name} atualizado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["clients"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível atualizar o cliente ${client?.name}`,
        description: error.request.response,
      });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: clientsService.delete,
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

  const isMutationLoading =
    createMutation.isPending ||
    updateMutation.isPending ||
    deleteMutation.isPending;

  const create: SubmitHandler<CreateClientSchema> = (data) =>
    createMutation.mutate({ data });

  const update = (id: string, data: UpdateClientSchema) =>
    updateMutation.mutate({ id, data });

  return {
    handleSubmit,
    register,
    errors,
    create,
    update,
    isMutationLoading,
    deleteMutation,
  };
};
