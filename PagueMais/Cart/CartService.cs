using System.Reflection.Metadata.Ecma335;
using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class CartService
  {
    private readonly ICartRepository _cartRepository;
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IPurchaseRepository purchaseRepository, IProductRepository productRepository)
    {
      _cartRepository = cartRepository;
      _purchaseRepository = purchaseRepository;
      _productRepository = productRepository;
    }

    //Criar novo Cart
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
      purchase.Total += product.Price;
      _purchaseRepository.Update(purchase);
      return cart;
    }

    //Aumentar produto de um Cart
    public void IncrementProductQuantity(Guid Id)
    {
      var cart = _cartRepository.FindById(Id) ?? throw new CartNotFoundException();
      var purchase = _purchaseRepository.FindById(cart.PurchaseId) ?? throw new PurchaseNotFoundException();
      var product = _productRepository.FindById(cart.ProductId) ?? throw new ProductNotFoundException();
      cart.Quantity += 1;
      purchase.Total += product.Price;
      _purchaseRepository.Update(purchase);
      _cartRepository.Update(cart);
    }

    //Diminuir Produto de um Cart
    public void DecrementProductQuantity(Guid Id)
    {
      var cart = _cartRepository.FindById(Id) ?? throw new CartNotFoundException();
      var purchase = _purchaseRepository.FindById(cart.PurchaseId) ?? throw new PurchaseNotFoundException();
      var product = _productRepository.FindById(cart.ProductId) ?? throw new ProductNotFoundException();

      cart.Quantity -= 1;

      if (cart.Quantity == 0)
      {
        RemoveProductFromCart(cart.Id);
        return;
      }

      purchase.Total -= product.Price;
      _purchaseRepository.Update(purchase);
      _cartRepository.Update(cart);
    }

    //Remover um Produto(Apagar Cart)
    public void RemoveProductFromCart(Guid Id)
    {
      var cart = _cartRepository.FindById(Id) ?? throw new CartNotFoundException();
      var purchase = _purchaseRepository.FindById(cart.PurchaseId) ?? throw new PurchaseNotFoundException();
      var product = _productRepository.FindById(cart.ProductId) ?? throw new ProductNotFoundException();

      purchase.Total -= product.Price * cart.Quantity;
      _purchaseRepository.Update(purchase);
      _cartRepository.Remove(cart);
    }

    public IEnumerable<Cart> GetAllCarts()
    {
      return _cartRepository.GetAllCarts();
    }
  }
}
