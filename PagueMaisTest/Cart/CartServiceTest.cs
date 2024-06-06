using Entities;
using Exceptions;
using Moq;
using Repositories;
using Services;
using Xunit;

namespace Tests
{
  public class CartServiceTest
  {
    private readonly Mock<ICartRepository> _cartRepository;
    private readonly Mock<IPurchaseRepository> _purchaseRepository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly CartService _cartService;

    public CartServiceTest()
    {
      _cartRepository = new Mock<ICartRepository>();
      _purchaseRepository = new Mock<IPurchaseRepository>();
      _productRepository = new Mock<IProductRepository>();
      _cartService = new CartService(_cartRepository.Object, _purchaseRepository.Object, _productRepository.Object);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenProductNotFound()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      _productRepository.Setup(repo => repo.FindById(productId)).Returns((Product)null);

      Assert.Throws<ProductNotFoundException>(() => _cartService.Create(productId, purchaseId));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenPurchaseNotFound()
    {
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _cartService.Create(productId, purchaseId));
    }

    [Fact]
    public void Create_ShouldCreateNewCart_WhenValid()
    {
      // Arrange
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      var purchase = new Purchase(0, EnumMethods.Credit, Guid.NewGuid());
      var cart = new Cart(productId, purchaseId, 1);

      _productRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns(product);
      _purchaseRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns(purchase);
      _purchaseRepository.Setup(repo => repo.Update(purchase));
      _cartRepository.Setup(repo => repo.FindByProductAndPurchaseId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Cart)null);
      _cartRepository.Setup(repo => repo.Create(cart)).Returns(cart);

      // Act
      Cart result = _cartService.Create(productId, purchaseId);

      _cartRepository.Verify(repo => repo.Create(cart), Times.Never);
      _purchaseRepository.Verify(repo => repo.Update(purchase), Times.Once);
    }

    [Fact]
    public void IncrementProductQuantity_ShouldThrowException_WhenCartNotFound()
    {
      var cartId = Guid.NewGuid();
      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.IncrementProductQuantity(cartId));
    }

    [Fact]
    public void IncrementProductQuantity_ShouldIncreaseQuantityAndTotal()
    {
      var cartId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      var cart = new Cart(productId, purchaseId, 1) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);
      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);

      _cartService.IncrementProductQuantity(cartId);

      Assert.Equal(2, cart.Quantity);
      Assert.Equal(200, purchase.Total);
      _cartRepository.Verify(repo => repo.Update(It.Is<Cart>(c => c.Quantity == 2)), Times.Once);
      _purchaseRepository.Verify(repo => repo.Update(It.Is<Purchase>(p => p.Total == 200)), Times.Once);
    }

    [Fact]
    public void DecrementProductQuantity_ShouldThrowException_WhenCartNotFound()
    {
      var cartId = Guid.NewGuid();
      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.DecrementProductQuantity(cartId));
    }

    [Fact]
    public void DecrementProductQuantity_ShouldDecreaseQuantityAndTotal()
    {
      var cartId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      var purchase = new Purchase(200, EnumMethods.Credit, Guid.NewGuid());
      var cart = new Cart(productId, purchaseId, 2) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);
      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);

      _cartService.DecrementProductQuantity(cartId);

      Assert.Equal(1, cart.Quantity);
      Assert.Equal(100, purchase.Total);
      _cartRepository.Verify(repo => repo.Update(It.Is<Cart>(c => c.Quantity == 1)), Times.Once);
      _purchaseRepository.Verify(repo => repo.Update(It.Is<Purchase>(p => p.Total == 100)), Times.Once);
    }

    [Fact]
    public void DecrementProductQuantity_ShouldRemoveCart_WhenQuantityIsZero()
    {
      var cartId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      var cart = new Cart(productId, purchaseId, 1) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);
      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);

      _cartService.DecrementProductQuantity(cartId);

      _purchaseRepository.Verify(repo => repo.Update(purchase), Times.Once);
      _cartRepository.Verify(repo => repo.Remove(cart), Times.Once);
    }

    [Fact]
    public void RemoveProductFromCart_ShouldThrowException_WhenCartNotFound()
    {
      var cartId = Guid.NewGuid();
      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns((Cart)null);

      Assert.Throws<CartNotFoundException>(() => _cartService.RemoveProductFromCart(cartId));
    }

    [Fact]
    public void RemoveProductFromCart_ShouldRemoveCartAndUpdateTotal()
    {
      var cartId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var purchaseId = Guid.NewGuid();
      var product = new Product("Test Product", 100);
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      var cart = new Cart(productId, purchaseId, 1) { Id = cartId };

      _cartRepository.Setup(repo => repo.FindById(cartId)).Returns(cart);
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);
      _productRepository.Setup(repo => repo.FindById(productId)).Returns(product);

      _cartService.RemoveProductFromCart(cartId);

      _cartRepository.Verify(repo => repo.Remove(It.Is<Cart>(c => c.Id == cartId)), Times.Once);
      _purchaseRepository.Verify(repo => repo.Update(It.Is<Purchase>(p => p.Total == 0)), Times.Once);
    }

    [Fact]
    public void GetAllCarts_ShouldReturnAllCarts()
    {
      var carts = new List<Cart>
            {
                new Cart(Guid.NewGuid(), Guid.NewGuid(), 1),
                new Cart(Guid.NewGuid(), Guid.NewGuid(), 2)
            };
      _cartRepository.Setup(repo => repo.GetAllCarts()).Returns(carts);

      var result = _cartService.GetAllCarts();

      Assert.Equal(carts, result);
    }
  }
}
