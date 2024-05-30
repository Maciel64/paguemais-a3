export type User = {
  id: string;
  name: string;
  email: string;
  cpf: string;
  phone: string;
  birthDate: string;
  createdAt: Date;
  updatedAt?: Date;
};

export interface CreateUserSchema {
  name: string;
  email: string;
  cpf: string;
  phone: string;
  birthDate: string;
}
