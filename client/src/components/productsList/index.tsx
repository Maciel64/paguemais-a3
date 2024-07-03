import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Product } from "@/types/product";
import { Button } from "../ui/button";
import { Cart, Purchase } from "@/types/purchase";
import { usePurchases } from "@/app/usePurchases";

interface ProductsListProps {
  products: Product[];
  carts: Cart[];
  purchase: Purchase;
  clientId: string;
}

const ProductsList = ({ products, carts, purchase }: ProductsListProps) => {
  const purchaseCarts = carts.filter((c) => c.purchaseId == purchase.id);
  const mappedProducts = products.map((product) => {
    const productCarts = purchaseCarts.filter(
      (cart) => cart.productId === product.id
    );

    const cart = productCarts.length > 0 ? productCarts[0] : null;
    return {
      ...product,
      cart,
    };
  });

  console.log(mappedProducts);

  const {
    createCartMutation,
    increaseQuantityMutation,
    decreaseQuantityMutation,
  } = usePurchases();

  return (
    <>
      {mappedProducts.map(({ id, cart, name, price }) => (
        <Card key={id}>
          <CardHeader>
            <CardTitle>{name}</CardTitle>
            <CardDescription>
              {price.toLocaleString("pt-br", {
                style: "currency",
                currency: "BRL",
              })}
            </CardDescription>
          </CardHeader>
          <CardContent className="flex gap-2">
            <Button
              size="sm"
              onClick={() =>
                cart ? decreaseQuantityMutation.mutate(cart.id) : null
              }
            >
              -
            </Button>
            {cart?.quantity ?? 0}
            <Button
              size="sm"
              onClick={() =>
                cart
                  ? increaseQuantityMutation.mutate(cart.id)
                  : createCartMutation.mutate({
                      purchaseId: purchase.id,
                      productId: id,
                    })
              }
            >
              +
            </Button>
          </CardContent>
        </Card>
      ))}
    </>
  );
};

export default ProductsList;
