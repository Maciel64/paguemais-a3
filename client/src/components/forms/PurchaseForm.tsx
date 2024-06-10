import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { PropsWithChildren } from "react";
import { Product } from "@/types/product";
import { useProductForm } from "./useProductForm";

interface PurchaseFormProps extends PropsWithChildren {
  product?: Product;
}

const PurchaseForm = ({ product }: PurchaseFormProps) => {
  const { handleSubmit, create, update, register, isMutationLoading } =
    useProductForm();

  return (
    <form
      className="flex flex-col gap-5"
      onSubmit={handleSubmit(
        product ? (data) => update(product?.id, data) : (data) => create(data)
      )}
    >
      <Input
        {...register("name")}
        type="text"
        placeholder="Nome"
        defaultValue={product?.name}
      />
      <Input
        {...register("price", { valueAsNumber: true })}
        type="number"
        placeholder="Preço"
        defaultValue={product?.price}
      />
      <Button disabled={isMutationLoading}>Concluir</Button>
    </form>
  );
};

export default PurchaseForm;
