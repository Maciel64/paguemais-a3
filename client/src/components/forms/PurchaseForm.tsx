import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { PropsWithChildren } from "react";
import { Product } from "@/types/product";
import { usePurchaseForm } from "./usePurchaseForm";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";

import { Controller } from "react-hook-form";

interface PurchaseFormProps extends PropsWithChildren {
  product?: Product;
}

const PurchaseForm = ({}) => {
  const {
    handleSubmit,
    create,
    register,
    isMutationLoading,
    clients,
    control,
  } = usePurchaseForm();

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
          <Select {...field}>
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

      <Select>
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
      <Button disabled={isMutationLoading}>Concluir</Button>
    </form>
  );
};

export default PurchaseForm;
