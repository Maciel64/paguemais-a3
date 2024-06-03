using Moq;
using Repositories;
using Services;

namespace Tests
{
  public class PurchaseServiceTest
  {
    private readonly Mock<IPurchaseRepository> _purchaseRepository;
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly PurchaseService _purchaseService;

    public PurchaseServiceTest()
    {
      _purchaseRepository = new Mock<IPurchaseRepository>();
      _clientRepository = new Mock<IClientRepository>();
      _purchaseService = new PurchaseService(_purchaseRepository.Object, _clientRepository.Object);
    }
  }
}