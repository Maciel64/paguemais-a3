using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Services;
using Entities;
using Repositories;
using Exceptions;

namespace Tests
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _clientService = new ClientService(_mockClientRepository.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateClient_WhenClientExists()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var updateDto = new UpdateClientDTO
            {
                Name = "Updated Name",
                Cpf = "12345678901",
                Email = "updated@example.com",
                Phone = 987654321,
                BirthDate = new DateTime(1990, 1, 1)
            };
            var existingClient = new Client
            {
                Id = clientId,
                Name = "Old Name",
                Cpf = "09876543210",
                Email = "old@example.com",
                Phone = 123456789,
                BirthDate = new DateTime(1980, 1, 1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockClientRepository.Setup(repo => repo.FindByIdAsync(clientId))
                .ReturnsAsync(existingClient);
            _mockClientRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>()))
                .Returns(Task.CompletedTask);

            // Act
            await _clientService.UpdateAsync(clientId, updateDto);

            // Assert
            _mockClientRepository.Verify(repo => repo.UpdateAsync(It.Is<Client>(client =>
                client.Id == clientId &&
                client.Name == updateDto.Name &&
                client.Cpf == updateDto.Cpf &&
                client.Email == updateDto.Email &&
                client.Phone == updateDto.Phone &&
                client.BirthDate == updateDto.BirthDate &&
                client.UpdatedAt > existingClient.UpdatedAt
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var updateDto = new UpdateClientDTO();

            _mockClientRepository.Setup(repo => repo.FindByIdAsync(clientId))
                .ReturnsAsync((Client)null);

            // Act & Assert
            await Assert.ThrowsAsync<ClientNotFoundException>(() =>
                _clientService.UpdateAsync(clientId, updateDto));
        }

        [Fact]
        public async Task GetClientByIdAsync_ShouldReturnClient_WhenClientExists()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var existingClient = new Client
            {
                Id = clientId,
                Name = "Client Name",
                Cpf = "12345678901",
                Email = "client@example.com",
                Phone = 123456789,
                BirthDate = new DateTime(1990, 1, 1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockClientRepository.Setup(repo => repo.FindByIdAsync(clientId))
                .ReturnsAsync(existingClient);

            // Act
            var client = await _clientService.GetClientByIdAsync(clientId);

            // Assert
            Assert.Equal(existingClient, client);
        }

        [Fact]
        public async Task GetClientByIdAsync_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            _mockClientRepository.Setup(repo => repo.FindByIdAsync(clientId))
                .ReturnsAsync((Client)null);

            // Act & Assert
            await Assert.ThrowsAsync<ClientNotFoundException>(() =>
                _clientService.GetClientByIdAsync(clientId));
        }
    }
}
