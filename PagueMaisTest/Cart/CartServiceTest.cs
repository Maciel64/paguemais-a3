using Moq;
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
  }
}