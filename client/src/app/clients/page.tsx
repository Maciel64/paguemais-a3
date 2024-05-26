"use client";

import { useQuery } from "@tanstack/react-query";
import { clientsService } from "../services/clients-service";

const Clients = () => {
  const { data, error, isLoading } = useQuery({
    queryKey: ["clients"],
    queryFn: clientsService.getAll,
  });

  console.log(data);

  return <h1>Clients</h1>;
};

export default Clients;
