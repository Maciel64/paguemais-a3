using Entities;
using Exceptions;
using Moq;
using Repositories;
using Services;
using Xunit;

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

    [Fact]
    public void GetAll_ShouldReturnAllPurchases()
    {
      var purchases = new List<Purchase> { new Purchase() };
      _purchaseRepository.Setup(repo => repo.GetAll()).Returns(purchases);

      var result = _purchaseService.GetAll();

      Assert.Equal(purchases, result);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenClientNotFound()
    {
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      _clientRepository.Setup(repo => repo.FindById(purchase.ClientId)).Returns((Client)null);

      Assert.Throws<ClientNotFoundException>(() => _purchaseService.Create(purchase));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenTotalIsNegative()
    {
      var client = new Client("Test", "12345678901", "test@example.com", 1234567890, DateTime.Now);
      var purchase = new Purchase(-1, EnumMethods.Credit, client.Id);
      _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns(client);

      Assert.Throws<PurchaseTotalIsInvalidException>(() => _purchaseService.Create(purchase));
    }

    [Fact]
    public void Create_ShouldReturnPurchase_WhenValid()
    {
      var client = new Client("Test", "12345678901", "test@example.com", 1234567890, DateTime.Now);
      var purchase = new Purchase(100, EnumMethods.Credit, client.Id);
      _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns(client);
      _purchaseRepository.Setup(repo => repo.Create(purchase)).Returns(purchase);

      var result = _purchaseService.Create(purchase);

      Assert.Equal(purchase, result);
    }

    [Fact]
    public void Remove_ShouldThrowException_WhenPurchaseNotFound()
    {
      var purchaseId = Guid.NewGuid();
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _purchaseService.Remove(purchaseId));
    }

    [Fact]
    public void Remove_ShouldCallRemove_WhenPurchaseExists()
    {
      var purchase = new Purchase();
      _purchaseRepository.Setup(repo => repo.FindById(purchase.Id)).Returns(purchase);

      _purchaseService.Remove(purchase.Id);

      _purchaseRepository.Verify(repo => repo.Remove(purchase), Times.Once);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenPurchaseNotFound()
    {
      var purchaseId = Guid.NewGuid();
      var updatedPurchase = new UpdatePurchaseDTO();
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _purchaseService.Update(purchaseId, updatedPurchase));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenTotalIsNegative()
    {
      var purchase = new Purchase();
      var updatedPurchase = new UpdatePurchaseDTO { Total = -1 };
      _purchaseRepository.Setup(repo => repo.FindById(purchase.Id)).Returns(purchase);

      Assert.Throws<PurchaseTotalIsInvalidException>(() => _purchaseService.Update(purchase.Id, updatedPurchase));
    }

    [Fact]
    public void Update_ShouldUpdatePurchase_WhenValid()
    {
      var purchase = new Purchase();
      var updatedPurchase = new UpdatePurchaseDTO { Total = 100, PaymentMethod = EnumMethods.Debit, Status = EnumStatus.Completed };
      _purchaseRepository.Setup(repo => repo.FindById(purchase.Id)).Returns(purchase);

      _purchaseService.Update(purchase.Id, updatedPurchase);

      _purchaseRepository.Verify(repo => repo.Update(It.Is<Purchase>(p => p.Total == 100 && p.PaymentMethod == EnumMethods.Debit && p.Status == EnumStatus.Completed)), Times.Once);
    }
  }
}
