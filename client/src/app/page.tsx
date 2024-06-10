"use client";

import { LoaderIcon, Pencil, Trash } from "lucide-react";

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
import ProductForm from "@/components/forms/ProductForm";
import { useProductForm } from "@/components/forms/useProductForm";
import { useProducts } from "./products/useProducts";
import { usePurchases } from "./usePurchases";
import PurchaseForm from "@/components/forms/PurchaseForm";
import ProductsList from "@/components/productsList";
import { Product } from "@/types/product";

const Products = () => {
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

  const { deleteMutation } = useProductForm();

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
  const tableHaveLeght = table?.getRowModel().rows?.length;

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

          {tableHaveLeght ? (
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
                    ) : cell.column.id == "total" ? (
                      (cell.getValue() as number).toLocaleString("pt-br", {
                        style: "currency",
                        currency: "BRL",
                      })
                    ) : cell.column.id == "createdAt" ? (
                      dateFormater.format(new Date(cell.getValue() as Date))
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
              ) : (
                <>Tem certeza que quer apagar o compra {purchase?.id}?</>
              )}
            </DialogTitle>

            {dialogUIState === "creating" ? (
              <PurchaseForm />
            ) : dialogUIState === "updating" ? (
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

export default Products;
