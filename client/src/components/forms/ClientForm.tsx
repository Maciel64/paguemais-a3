import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { PropsWithChildren } from "react";
import { Client } from "@/types/client";
import { useClientForm } from "./useClientForm";

interface ClientFormProps extends PropsWithChildren {
  client?: Client;
}

const ClientForm = ({ client }: ClientFormProps) => {
  const { handleSubmit, create, update, register, isMutationLoading } =
    useClientForm();

  return (
    <form
      className="flex flex-col gap-5"
      onSubmit={handleSubmit(
        client ? (data) => update(client?.id, data) : (data) => create(data)
      )}
    >
      <Input
        {...register("name")}
        type="text"
        placeholder="Nome"
        defaultValue={client?.name}
      />
      <Input
        {...register("email")}
        type="text"
        placeholder="Email"
        defaultValue={client?.email}
      />
      <Input
        {...register("cpf")}
        type="text"
        placeholder="Cpf"
        defaultValue={client?.cpf}
      />
      <Input
        {...register("phone")}
        type="number"
        placeholder="Telefone"
        defaultValue={client?.phone}
      />
      <Input
        {...register("birthDate")}
        type="date"
        placeholder="Data de nascimento"
        defaultValue={client?.birthDate.substring(0, 10)}
      />

      <Button disabled={isMutationLoading}>Concluir</Button>
    </form>
  );
};

export default ClientForm;
