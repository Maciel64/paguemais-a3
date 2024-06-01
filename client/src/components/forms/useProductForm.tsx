import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { toast } from "../ui/use-toast";
import { AxiosError } from "axios";
import { useUI } from "@/app/products/useProducts";
import { productsService } from "@/services/products-service";
import { CreateProductSchema, UpdateProductSchema } from "@/types/product";

const createProductSchema = z.object({
  name: z.string(),
  price: z.number(),
});

type typeCreateProductSchema = z.infer<typeof createProductSchema>;

export const useProductForm = () => {
  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<typeCreateProductSchema>({
    resolver: zodResolver(createProductSchema),
  });

  const { closeDialog, product } = useUI();

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: productsService.create,
    onSuccess: () => {
      toast({
        title: "Sucesso!",
      });
      queryClient.invalidateQueries({ queryKey: ["products"] });
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
    mutationFn: productsService.update,
    onSuccess: () => {
      toast({
        title: `Produto ${product?.name} atualizado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["products"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível atualizar o produto ${product?.name}`,
        description: error.request.response,
      });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: productsService.delete,
    onSuccess: () => {
      toast({
        title: `Produto ${product?.name} deletado com sucesso!`,
      });
      queryClient.invalidateQueries({ queryKey: ["products"] });
      closeDialog();
    },
    onError: (error: AxiosError) => {
      toast({
        title: `Não foi possível apagar o produto ${product?.name}`,
        description: error.request.response,
      });
    },
  });

  const isMutationLoading =
    createMutation.isPending ||
    updateMutation.isPending ||
    deleteMutation.isPending;

  const create: SubmitHandler<CreateProductSchema> = (data) =>
    createMutation.mutate({ data });

  const update = (id: string, data: UpdateProductSchema) =>
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
