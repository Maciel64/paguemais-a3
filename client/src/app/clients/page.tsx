"use client";

import { flexRender } from "@tanstack/react-table";
import { LoaderIcon } from "lucide-react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";

import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

import { Input } from "@/components/ui/input";

import { Pencil, Trash } from "lucide-react";

import { useClients } from "./useClients";

const Clients = () => {
  const { create, handleSubmit, isLoading, register, columns, table } =
    useClients();

  const tableHaveLeght = table?.getRowModel().rows?.length;
  const dateFormater = new Intl.DateTimeFormat("pt-BR");

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
                      <Button variant="outline">
                        <Pencil />
                      </Button>
                    ) : cell.column.id == "delete" ? (
                      <Button variant="outline">
                        <Trash />
                      </Button>
                    ) : cell.column.id == "birthDate" ? (
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

      <Dialog>
        <DialogTrigger asChild>
          <Button className="mt-4">Novo</Button>
        </DialogTrigger>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Crie um novo Cliente</DialogTitle>
          </DialogHeader>

          <form className="flex flex-col gap-5" onSubmit={handleSubmit(create)}>
            <Input {...register("name")} type="text" placeholder="Nome" />
            <Input {...register("email")} type="text" placeholder="Email" />
            <Input {...register("cpf")} type="text" placeholder="Cpf" />
            <Input
              {...register("phone")}
              type="number"
              placeholder="Telefone"
            />
            <Input
              {...register("birthDate")}
              type="date"
              placeholder="Data de nascimento"
            />

            <Button>Concluir</Button>
          </form>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default Clients;
