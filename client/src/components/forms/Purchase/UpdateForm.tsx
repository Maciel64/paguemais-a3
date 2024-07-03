import { Button } from "@/components/ui/button";
import { Input } from "../../ui/input";
import { PropsWithChildren, useEffect } from "react";
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
import { useUpdateForm } from "./useUpdateForm";

interface PurchaseFormProps extends PropsWithChildren {
  purchase: Purchase;
}

const UpdateForm = ({ purchase }: PurchaseFormProps) => {
  const { handleSubmit, update, isMutationLoading, clients, control, reset } =
    useUpdateForm();

  useEffect(() => {
    purchase &&
      reset({
        paymentMethod: purchase.paymentMethod.toString(),
        status: purchase.status.toString(),
      });
  }, []);

  return (
    <form
      className="flex flex-col gap-5"
      onSubmit={handleSubmit((data) =>
        update(purchase.id, {
          paymentMethod: Number(data.paymentMethod),
          status: Number(data.status),
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
            defaultValue={purchase.paymentMethod.toString()}
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
        name="status"
        control={control}
        render={({ field }) => (
          <Select
            onValueChange={(value) =>
              field.onChange({ target: { name, value } })
            }
            defaultValue={purchase.status.toString()}
            {...field}
          >
            <SelectTrigger>
              <SelectValue placeholder="Estatus" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="0">Em andamento</SelectItem>
              <SelectItem value="1">Completo</SelectItem>
              <SelectItem value="2">Cancelado</SelectItem>
            </SelectContent>
          </Select>
        )}
      />

      <Button disabled={isMutationLoading}>Concluir</Button>
    </form>
  );
};

export default UpdateForm;
