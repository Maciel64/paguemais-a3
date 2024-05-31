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

import { Pencil, Trash } from "lucide-react";

import { useClients } from "./useClients";
import ClientForm from "@/components/forms/ClientForm";
import { useClientForm } from "@/components/forms/useClientForm";

const Clients = () => {
  const {
    isLoading,
    columns,
    table,
    client,
    setClient,
    dialogUIState,
    setDialogUIState,
    closeDialog,
  } = useClients();

  const { deleteMutation } = useClientForm();

  console.log(client);

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
                      <Button
                        variant="outline"
                        onClick={() => {
                          setDialogUIState("updating");
                          setClient(cell.row.original);
                        }}
                      >
                        <Pencil />
                      </Button>
                    ) : cell.column.id == "delete" ? (
                      <Button
                        variant="outline"
                        onClick={() => {
                          setDialogUIState("deleting");
                          setClient(cell.row.original);
                        }}
                      >
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
                <>Atualizando o cliente {client?.name}</>
              ) : dialogUIState === "creating" ? (
                <>Crie um novo Cliente</>
              ) : (
                <>Tem certeza que quer apagar o cliente {client?.name}?</>
              )}
            </DialogTitle>

            {dialogUIState === "updating" || dialogUIState === "creating" ? (
              <ClientForm client={client ?? undefined} />
            ) : (
              <Button
                onClick={() => deleteMutation.mutate(client?.id as string)}
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

export default Clients;
