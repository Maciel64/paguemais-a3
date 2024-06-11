import { Button } from "@/components/ui/button";
import { Input } from "../../ui/input";
import { PropsWithChildren } from "react";
import { Product } from "@/types/product";
import { useCreateForm } from "./useCreateForm";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../ui/select";

import { Controller } from "react-hook-form";
import { Purchase, UpdatePurchaseSchema } from "@/types/purchase";

interface PurchaseFormProps extends PropsWithChildren {
  purchase?: Purchase;
}

const CreateForm = ({ purchase }: PurchaseFormProps) => {
  const { handleSubmit, create, update, isMutationLoading, clients, control } =
    useCreateForm();

  return (
    <form
      className="flex flex-col gap-5"
      onSubmit={handleSubmit((data) =>
        create({
          clientId: data.clientId,
          paymentMethod: Number(data.paymentMethod),
        })
      )}
    >
      <Controller
        name="paymentMethod"
        control={control}
        render={({ field }) => (
          <Select
            onValueChange={(value) =>
              field.onChange({ target: { name, value } })
            }
            {...field}
          >
            <SelectTrigger>
              <SelectValue placeholder="Método de pagamento" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="0">Crédito</SelectItem>
              <SelectItem value="1">Débido</SelectItem>
              <SelectItem value="2">Pix</SelectItem>
              <SelectItem value="3">Dinheiro</SelectItem>
            </SelectContent>
          </Select>
        )}
      />

      <Controller
        name="clientId"
        control={control}
        render={({ field }) => (
          <Select
            onValueChange={(value) =>
              field.onChange({ target: { name, value } })
            }
            {...field}
          >
            <SelectTrigger>
              <SelectValue placeholder="Cliente" />
            </SelectTrigger>
            <SelectContent>
              {clients.map(({ name, id }) => (
                <SelectItem key={id} value={id}>
                  {name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        )}
      />

      <Button disabled={isMutationLoading}>Concluir</Button>
    </form>
  );
};

export default CreateForm;
