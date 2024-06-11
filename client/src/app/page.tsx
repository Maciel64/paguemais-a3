"use client";

import { LoaderIcon, Pencil, ShoppingCart, Trash } from "lucide-react";

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { flexRender } from "@tanstack/react-table";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { usePurchases } from "./usePurchases";
import CreatePurchaseForm from "@/components/forms/Purchase/CreateForm";
import UpdatePurchaseForm from "@/components/forms/Purchase/UpdateForm";
import ProductsList from "@/components/productsList";
import { Product } from "@/types/product";
import { Purchase } from "@/types/purchase";

const Purchases = () => {
  const {
    columns,
    table,
    isLoading,
    dialogUIState,
    setDialogUIState,
    closeDialog,
    purchase,
    client,
    product,
    setIsDeleting,
    setIsUpdating,
    deleteMutation,
    paymentMethodMapper,
  } = usePurchases();

  const products: Product[] = [
    {
      id: "oppoa",
      name: "Teste",
      price: 12,
      createdAt: new Date(),
    },
    {
      id: "oppoa",
      name: "Teste",
      price: 12,
      createdAt: new Date(),
    },
    {
      id: "oppoa",
      name: "Teste",
      price: 12,
      createdAt: new Date(),
    },
    {
      id: "oppoa",
      name: "Teste",
      price: 12,
      createdAt: new Date(),
    },
  ];

  const dateFormater = new Intl.DateTimeFormat("pt-BR", {
    year: "numeric",
    month: "long",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
  });
  const shortDateFormater = new Intl.DateTimeFormat("pt-BR", {
    year: "numeric",
    month: "numeric",
    day: "numeric",
    hour: "numeric",
    minute: "numeric",
  });
  const tableHaveLenght = table?.getRowModel().rows?.length;

  return isLoading ? (
    <LoaderIcon className="animate-spin" />
  ) : (
    <>
      <Table>
        <TableHeader>
          {table.getHeaderGroups().map((group) => (
            <TableRow key={group.id}>
              {group.headers.map((head) => (
                <TableHead key={head.id}>
                  {head.isPlaceholder
                    ? null
                    : flexRender(
                        head.column.columnDef.header,
                        head.getContext()
                      )}
                </TableHead>
              ))}
            </TableRow>
          ))}
        </TableHeader>

        <TableBody>
          {isLoading && (
            <TableRow>
              <LoaderIcon className="animate-spin" />
            </TableRow>
          )}

          {tableHaveLenght ? (
            table.getRowModel().rows.map((row) => (
              <TableRow key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <TableCell key={cell.id}>
                    {cell.column.id == "update" ? (
                      <Button
                        variant="outline"
                        onClick={() => setIsUpdating(cell.row.original)}
                      >
                        <Pencil />
                      </Button>
                    ) : cell.column.id == "delete" ? (
                      <Button
                        variant="outline"
                        onClick={() => setIsDeleting(cell.row.original)}
                      >
                        <Trash />
                      </Button>
                    ) : cell.column.id == "paymentMethod" ? (
                      paymentMethodMapper[cell.getValue() as number]
                    ) : cell.column.id == "total" ? (
                      (cell.getValue() as number).toLocaleString("pt-br", {
                        style: "currency",
                        currency: "BRL",
                      })
                    ) : cell.column.id == "createdAt" ? (
                      dateFormater.format(new Date(cell.getValue() as Date))
                    ) : cell.column.id == "products" ? (
                      <Button
                        variant="outline"
                        onClick={() => setDialogUIState("addingProducts")}
                      >
                        <ShoppingCart />
                      </Button>
                    ) : (
                      (cell.getValue() as string)
                    )}
                  </TableCell>
                ))}
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={columns.length} className="h-24 text-center">
                No results.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>

      <Dialog open={dialogUIState !== "closed"}>
        <DialogTrigger asChild>
          <Button className="mt-4" onClick={() => setDialogUIState("creating")}>
            Novo
          </Button>
        </DialogTrigger>
        <DialogContent onInteractOutside={closeDialog}>
          <DialogHeader>
            <DialogTitle>
              {dialogUIState === "updating" ? (
                <>
                  {shortDateFormater.format(
                    new Date(purchase?.createdAt as Date)
                  )}
                  : {client?.id}
                </>
              ) : dialogUIState === "creating" ? (
                <>Crie uma nova compra</>
              ) : dialogUIState === "addingProducts" ? (
                <>Escolha os produtos</>
              ) : (
                <>Tem certeza que quer apagar o compra {purchase?.id}?</>
              )}
            </DialogTitle>

            {dialogUIState === "creating" ? (
              <CreatePurchaseForm />
            ) : dialogUIState === "updating" ? (
              <UpdatePurchaseForm purchase={purchase as Purchase} />
            ) : dialogUIState === "addingProducts" ? (
              <div className="grid grid-cols-3 gap-2">
                <ProductsList products={products} />
              </div>
            ) : (
              <Button
                onClick={() => deleteMutation.mutate(purchase?.id as string)}
              >
                Confirmar
              </Button>
            )}
          </DialogHeader>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default Purchases;
