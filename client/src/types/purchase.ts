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
  clientId: string;
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
  productId: string;
  product: Product;
  purchaseId: string;
  purchase: Purchase;
  createdAt: Date;
  updatedAt?: Date;
};

export interface CreateCartSchema {
  productId: string;
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
  clientId: string | null;
  setPurchase: (purchase: Purchase | null) => void;
  setProduct: (product: Product | null) => void;
  setClientId: (client: string | null) => void;
  closeDialog: () => void;
  setIsDeleting: (purchase: Purchase) => void;
  setIsUpdating: (purchase: Purchase) => void;
  setIsAddingProducts: () => void;
}
