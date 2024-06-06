using System;
using System.Collections.Generic;
using Entities;
using Exceptions;
using Moq;
using Repositories;
using Services;
using Xunit;

namespace Tests
{
  public class PurchaseServiceTests
  {
    private readonly Mock<IPurchaseRepository> _purchaseRepository;
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly Mock<ICartRepository> _cartRepository;
    private readonly PurchaseService _purchaseService;

    public PurchaseServiceTests()
    {
      _purchaseRepository = new Mock<IPurchaseRepository>();
      _clientRepository = new Mock<IClientRepository>();
      _cartRepository = new Mock<ICartRepository>();
      _purchaseService = new PurchaseService(_purchaseRepository.Object, _clientRepository.Object, _cartRepository.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnAllPurchases()
    {
      var purchases = new List<Purchase>
            {
                new Purchase(100, EnumMethods.Credit, Guid.NewGuid()),
                new Purchase(200, EnumMethods.Debit, Guid.NewGuid())
            };
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
    public void Create_ShouldThrowException_WhenTotalIsInvalid()
    {
      var purchase = new Purchase(-100, EnumMethods.Credit, Guid.NewGuid());
      _clientRepository.Setup(repo => repo.FindById(purchase.ClientId)).Returns(new Client("Test Client", "12345678901", "test@test.com", 1234567890, DateTime.UtcNow));

      Assert.Throws<PurchaseTotalIsInvalidException>(() => _purchaseService.Create(purchase));
    }

    [Fact]
    public void Create_ShouldReturnPurchase_WhenValid()
    {
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      _clientRepository.Setup(repo => repo.FindById(purchase.ClientId)).Returns(new Client("Test Client", "12345678901", "test@test.com", 1234567890, DateTime.UtcNow));
      _purchaseRepository.Setup(repo => repo.Create(purchase)).Returns(purchase);

      var result = _purchaseService.Create(purchase);

      Assert.Equal(purchase, result);
      _purchaseRepository.Verify(repo => repo.Create(purchase), Times.Once);
    }

    [Fact]
    public void Remove_ShouldThrowException_WhenPurchaseNotFound()
    {
      var purchaseId = Guid.NewGuid();
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _purchaseService.Remove(purchaseId));
    }

    [Fact]
    public void Remove_ShouldRemovePurchase_WhenValid()
    {
      var purchaseId = Guid.NewGuid();
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);

      _purchaseService.Remove(purchaseId);

      _purchaseRepository.Verify(repo => repo.Remove(purchase), Times.Once);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenPurchaseNotFound()
    {
      var purchaseId = Guid.NewGuid();
      var updatedPurchase = new UpdatePurchaseDTO { Total = 150, PaymentMethod = EnumMethods.Debit, Status = EnumStatus.Completed };
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _purchaseService.Update(purchaseId, updatedPurchase));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenTotalIsInvalid()
    {
      var purchaseId = Guid.NewGuid();
      var updatedPurchase = new UpdatePurchaseDTO { Total = -150 };
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);

      Assert.Throws<PurchaseTotalIsInvalidException>(() => _purchaseService.Update(purchaseId, updatedPurchase));
    }

    [Fact]
    public void Update_ShouldUpdatePurchase_WhenValid()
    {
      var purchaseId = Guid.NewGuid();
      var updatedPurchase = new UpdatePurchaseDTO { Total = 150, PaymentMethod = EnumMethods.Debit, Status = EnumStatus.Completed };
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid()) { Id = purchaseId };
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);

      _purchaseService.Update(purchaseId, updatedPurchase);

      Assert.Equal(150, purchase.Total);
      Assert.Equal(EnumMethods.Debit, purchase.PaymentMethod);
      Assert.Equal(EnumStatus.Completed, purchase.Status);
      _purchaseRepository.Verify(repo => repo.Update(purchase), Times.Once);
    }

    [Fact]
    public void GetPurchaseById_ShouldThrowException_WhenPurchaseNotFound()
    {
      var purchaseId = Guid.NewGuid();
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns((Purchase)null);

      Assert.Throws<PurchaseNotFoundException>(() => _purchaseService.GetPurchaseById(purchaseId));
    }

    [Fact]
    public void GetPurchaseById_ShouldReturnPurchase_WhenValid()
    {
      var purchaseId = Guid.NewGuid();
      var purchase = new Purchase(100, EnumMethods.Credit, Guid.NewGuid());
      _purchaseRepository.Setup(repo => repo.FindById(purchaseId)).Returns(purchase);

      var result = _purchaseService.GetPurchaseById(purchaseId);

      Assert.Equal(purchase, result);
    }
  }
}
