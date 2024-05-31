export type Client = {
  id: string;
  name: string;
  email: string;
  cpf: string;
  phone: string;
  birthDate: string;
  createdAt: Date;
  updatedAt?: Date;
};

export interface CreateClientSchema {
  name: string;
  email: string;
  cpf: string;
  phone: string;
  birthDate: string;
}

export interface UpdateClientSchema extends Partial<CreateClientSchema> {}
