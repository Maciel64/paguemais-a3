export type Product = {
  id: string;
  name: string;
  price: number;
  createdAt: Date;
  updatedAt?: Date;
};

export interface CreateProductSchema {
  name: string;
  price: number;
}

export interface UpdateProductSchema extends Partial<CreateProductSchema> {}

export interface UseProductState {
  product: Product | null;
  setProduct: (product: Product | null) => void;
  closeDialog: () => void;
  setIsDeleting: (product: Product) => void;
  setIsUpdating: (product: Product) => void;
}
