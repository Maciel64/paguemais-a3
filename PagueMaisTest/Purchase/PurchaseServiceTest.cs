using Xunit;
using Moq;
using Services;
using Entities;
using Exceptions;
using Repositories;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class PurchaseServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnAllPurchases()
        {
            // Arrange
            var purchases = new List<Purchase>
            {
                new Purchase { Id = Guid.NewGuid(), Total = 100, PaymentMethod = EnumMethods.Credit },
                new Purchase { Id = Guid.NewGuid(), Total = 200, PaymentMethod = EnumMethods.Debit }
            };

            var mockPurchaseRepository = new Mock<IPurchaseRepository>();
            mockPurchaseRepository.Setup(repo => repo.GetAll()).Returns(purchases);

            var purchaseService = new PurchaseService(mockPurchaseRepository.Object, null);

            // Act
            var result = purchaseService.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Create_ValidPurchase_ShouldAddNewPurchase()
        {
            // Arrange
            var purchase = new Purchase { Total = 100, PaymentMethod = EnumMethods.Credit, ClientId = Guid.NewGuid() };

            var mockClientRepository = new Mock<IClientRepository>();
            mockClientRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns(new Client());

            var mockPurchaseRepository = new Mock<IPurchaseRepository>();
            mockPurchaseRepository.Setup(repo => repo.Create(It.IsAny<Purchase>())).Returns(purchase);

            var purchaseService = new PurchaseService(mockPurchaseRepository.Object, mockClientRepository.Object);

            // Act
            var createdPurchase = purchaseService.Create(purchase);

            // Assert
            Assert.NotNull(createdPurchase);
        }

        [Fact]
        public void Create_InvalidTotal_ShouldThrowException()
        {
            // Arrange
            var purchase = new Purchase { Total = -100, PaymentMethod = EnumMethods.Credit, ClientId = Guid.NewGuid() };

            var mockClientRepository = new Mock<IClientRepository>();
            mockClientRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns(new Client());

            var mockPurchaseRepository = new Mock<IPurchaseRepository>();

            var purchaseService = new PurchaseService(mockPurchaseRepository.Object, mockClientRepository.Object);

            // Act & Assert
            Assert.Throws<PurchaseTotalIsInvalidException>(() => purchaseService.Create(purchase));
        }

        // Testes para os outros métodos...
    }
}