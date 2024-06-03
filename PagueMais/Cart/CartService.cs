using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class CartService(CartRepository cartRepository, PurchaseRepository purchaseRepository, IProductRepository productRepository)
  {
    private readonly CartRepository _cartRepository = cartRepository;
    private readonly PurchaseRepository _purchaseRepository = purchaseRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public Cart Create(Guid ProductId, Guid PurchaseId)
    {
      var product = _productRepository.FindById(ProductId) ?? throw new ProductNotFoundException();
      var purchase = _purchaseRepository.FindById(PurchaseId) ?? throw new PurchaseNotFoundException();

      var alreadyCreatedCart = _cartRepository.FindByProductAndPurchaseId(ProductId, PurchaseId);

      if (alreadyCreatedCart is not null)
      {
        IncrementProductQuantity(alreadyCreatedCart.ProductId);
        return alreadyCreatedCart;
      }

      var cart = new Cart(product.Id, purchase.Id, 1);

      _cartRepository.Create(cart);

      return cart;
    }

    public void IncrementProductQuantity(Guid Id)
    {
      var Cart = _cartRepository.FindById(Id) ?? throw new CartNotFoundException();

      Cart.Quantity += 1;

      _cartRepository.Update(Cart);
    }

    public void DecrementProductQuantity(Guid Id)
    {
      var Cart = _cartRepository.FindById(Id) ?? throw new CartNotFoundException();

      if (Cart.Quantity == 0)
      {
        throw new CartQuantityIsZeroException();
      }

      Cart.Quantity += 1;

      _cartRepository.Update(Cart);
    }
  }
}