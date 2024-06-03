using Moq;
using Xunit;
using Entities;
using Exceptions;
using Repositories;
using Services;

namespace Tests
{
  public class CartServiceTest
  {
    private readonly Mock<ICartRepository> _cartRepository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly Mock<IPurchaseRepository> _purchaseRepository;
    private readonly CartService _cartService;

    public CartServiceTest()
    {
      _cartRepository = new Mock<ICartRepository>();
      _productRepository = new Mock<IProductRepository>();
      _purchaseRepository = new Mock<IPurchaseRepository>();
      _cartService = new CartService(_cartRepository.Object, _purchaseRepository.Object, _productRepository.Object);
    }

    [Fact]
    public void Create_WhenProductAndPurchaseExist_ShouldCreateCart()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 10.0f) { Id = productId };
      var purchase = new Purchase { Id = purchaseId };

      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);
      _cartRepository.Setup(repo => repo.FindByProductAndPurchaseId(productId, purchaseId)).Returns((Cart)null);

      var cart = _cartService.Create(productId, purchaseId);

      Assert.NotNull(cart);
      Assert.Equal(productId, cart.ProductId);
      Assert.Equal(purchaseId, cart.PurchaseId);
      Assert.Equal(1, cart.Quantity);
      _cartRepository.Verify(repo => repo.Create(It.IsAny<Cart>()), Times.Once);
    }

    [Fact]
    public void Create_WhenCartAlreadyExists_ShouldIncrementQuantity()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var existingCart = new Cart(productId, purchaseId, 1) { Id = Guid.NewGuid() };

      _productRepository.Setup(repo => repo.FindById(productId)).Returns(new Product("Test Product", 10.0f) { Id = productId });
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(new Purchase { Id = purchaseId });
      _cartRepository.Setup(repo => repo.FindByProductAndPurchaseId(productId, purchaseId)).Returns(existingCart);

      var cart = _cartService.Create(productId, purchaseId);

      Assert.NotNull(cart);
      Assert.Equal(productId, cart.ProductId);
      Assert.Equal(purchaseId, cart.PurchaseId);
      Assert.Equal(2, cart.Quantity);
      _cartRepository.Verify(repo => repo.Update(It.IsAny<Cart>()), Times.Once);
    }

    [Fact]
    public void IncrementProductQuantity_WhenCartExists_ShouldIncrementQuantity()
    {
      var cartId = Guid.NewGuid();
      var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), 1) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);

      _cartService.IncrementProductQuantity(cartId);

      Assert.Equal(2, cart.Quantity);
      _cartRepository.Verify(repo => repo.Update(cart), Times.Once);
    }

    [Fact]
    public void IncrementProductQuantity_WhenCartNotFound_ShouldThrowException()
    {
      var cartId = Guid.NewGuid();

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.IncrementProductQuantity(cartId));
    }

    [Fact]
    public void DecrementProductQuantity_WhenQuantityIsZero_ShouldThrowException()
    {
      var cartId = Guid.NewGuid();
      var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), 0) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);

      Assert.Throws<CartQuantityIsZeroException>(() => _cartService.DecrementProductQuantity(cartId));
    }

    [Fact]
    public void DecrementProductQuantity_WhenCartExists_ShouldDecrementQuantity()
    {
      var cartId = Guid.NewGuid();
      var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), 2) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);

      _cartService.DecrementProductQuantity(cartId);

      Assert.Equal(1, cart.Quantity);
      _cartRepository.Verify(repo => repo.Update(cart), Times.Once);
    }

    [Fact]
    public void DecrementProductQuantity_WhenCartNotFound_ShouldThrowException()
    {
      var cartId = Guid.NewGuid();

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.DecrementProductQuantity(cartId));
    }
  }
}