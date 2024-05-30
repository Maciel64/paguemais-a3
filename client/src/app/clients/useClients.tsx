import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { clientsService } from "../services/clients-service";
import { SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "@/components/ui/use-toast";
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { CreateUserSchema, User } from "@/types/user";
import { useMemo } from "react";
import { AxiosError } from "axios";

const createClientSchema = z.object({
  name: z.string(),
  email: z.string().email(),
  phone: z.string(),
  cpf: z.string(),
  birthDate: z.string(),
});

type typeCreateClientSchema = z.infer<typeof createClientSchema>;

export const useClients = () => {
  const columns: ColumnDef<User>[] = useMemo(
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

  const queryClient = useQueryClient();

  const {
    handleSubmit,
    register,
    formState: { errors },
  } = useForm<typeCreateClientSchema>({
    resolver: zodResolver(createClientSchema),
  });

  const createMutation = useMutation({
    mutationFn: clientsService.create,
    onSuccess: () => {
      toast({
        title: "Sucesso!",
      });
      queryClient.invalidateQueries({ queryKey: ["clients"] });
    },
    onError: (error: AxiosError) => {
      toast({
        title: "Não foi possível criar um novo usuário",
        description: error.request.response,
      });
    },
  });

  const create: SubmitHandler<CreateUserSchema> = (data) =>
    createMutation.mutate(data);

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return {
    create,
    handleSubmit,
    register,
    table,
    isLoading,
    columns,
  };
};
