using Moq;
using Services;
using Repositories;
using Entities;
using Exceptions;
using Xunit;

namespace Tests
{
  public class CartServiceTests
  {
    private readonly CartService _cartService;
    private readonly Mock<ICartRepository> _mockCartRepository = new();
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository = new();
    private readonly Mock<IProductRepository> _mockProductRepository = new();

    public CartServiceTests()
    {
      _cartService = new CartService(_mockCartRepository.Object, _mockPurchaseRepository.Object, _mockProductRepository.Object);
    }

    [Fact]
    public void Create_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();

      _mockProductRepository.Setup(repo => repo.FindById(productId)).Returns((Product)null);

      Assert.Throws<ProductNotFoundException>(() => _cartService.Create(productId, purchaseId));
    }

    [Fact]
    public void Create_ShouldThrowPurchaseNotFoundException_WhenPurchaseDoesNotExist()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();

      _mockProductRepository.Setup(repo => repo.FindById(productId)).Returns(new Product("TestProduct", 10.0f));
      _mockPurchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _cartService.Create(productId, purchaseId));
    }

    [Fact]
    public void IncrementProductQuantity_ShouldThrowCartNotFoundException_WhenCartDoesNotExist()
    {
      var cartId = Guid.NewGuid();

      _mockCartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.IncrementProductQuantity(cartId));
    }

    [Fact]
    public void DecrementProductQuantity_ShouldThrowCartNotFoundException_WhenCartDoesNotExist()
    {
      var cartId = Guid.NewGuid();

      _mockCartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.DecrementProductQuantity(cartId));
    }

    [Fact]
    public void DecrementProductQuantity_ShouldThrowCartQuantityIsZeroException_WhenCartQuantityIsZero()
    {
      var cartId = Guid.NewGuid();
      var cart = new Cart(Guid.NewGuid(), Guid.NewGuid(), 0);

      _mockCartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);

      Assert.Throws<CartQuantityIsZeroException>(() => _cartService.DecrementProductQuantity(cartId));
    }
  }
}