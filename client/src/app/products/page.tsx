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
import { useProducts } from "./useProducts";
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

const Products = () => {
  const {
    columns,
    table,
    isLoading,
    dialogUIState,
    setDialogUIState,
    closeDialog,
    product,
    setIsDeleting,
    setIsUpdating,
  } = useProducts();

  const { deleteMutation } = useProductForm();

  const dateFormater = new Intl.DateTimeFormat("pt-BR", {
    year: "numeric",
    month: "long",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
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
                    ) : cell.column.id == "createdAt" ? (
                      dateFormater.format(
                        new Date(cell.row.original.createdAt as Date)
                      )
                    ) : cell.column.id == "price" ? (
                      cell.row.original.price.toLocaleString("pt-br", {
                        style: "currency",
                        currency: "BRL",
                      })
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
                <>Atualizando o cliente {product?.name}</>
              ) : dialogUIState === "creating" ? (
                <>Crie um novo Produto</>
              ) : (
                <>Tem certeza que quer apagar o produto {product?.name}?</>
              )}
            </DialogTitle>

            {dialogUIState === "updating" || dialogUIState === "creating" ? (
              <ProductForm product={product ?? undefined} />
            ) : (
              <Button
                onClick={() => deleteMutation.mutate(product?.id as string)}
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
