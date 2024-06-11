import { Client } from "./client";
import { Product } from "./product";
import { DialogUIStates } from "./ui";

enum PaymentMethods {
  Credit,
  Debit,
  Pix,
  Money,
}

enum Status {
  Pending,
  Completed,
  Canceled,
}

export type Purchase = {
  id: string;
  total: number;
  paymentMethod: PaymentMethods;
  status: Status;
  createdAt: Date;
  finishedAt: Date;
  updatedAt?: Date;
  carts?: Cart[];
};

export interface CreatePurchaseSchema {
  paymentMethod: PaymentMethods;
  clientId: string;
}

export interface UpdatePurchaseSchema {
  paymentMethod: PaymentMethods;
  status: Status;
}

export type Cart = {
  id: string;
  quantity: number;
  product: Product;
  purchase: Purchase;
  createdAt: Date;
  updatedAt?: Date;
};

export interface CreateCartSchema {
  clientId: string;
  purchaseId: string;
}

export type PurchaseDialogUIStates = DialogUIStates | "addingProducts";

export interface UsePurchaseUIState {
  dialogUIState: PurchaseDialogUIStates;
  setDialogUIState: (UIState: PurchaseDialogUIStates) => void;
}

export interface UsePurchaseState {
  purchase: Purchase | null;
  product: Product | null;
  client: Client | null;
  setPurchase: (purchase: Purchase | null) => void;
  setProduct: (product: Product | null) => void;
  setClient: (client: Client | null) => void;
  closeDialog: () => void;
  setIsDeleting: (purchase: Purchase) => void;
  setIsUpdating: (purchase: Purchase) => void;
  setIsAddingProducts: () => void;
}
