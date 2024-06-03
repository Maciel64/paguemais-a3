using Moq;
using Repositories;
using Services;

namespace Tests
{
  public class ClientServiceTest
  {
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly ClientService _clientService;

    public ClientServiceTest()
    {
      _clientRepository = new Mock<IClientRepository>();
      _clientService = new ClientService(_clientRepository.Object);
    }
  [Fact]
        public void ShouldGetAllClients()
        {
            var mockClients = new List<Client>
            {
                new Client("Client 1", "client1@example.com", "12345678901", 123456789),
                new Client("Client 2", "client2@example.com", "09876543210", 987654321)
            };

            _clientRepository.Setup(repo => repo.GetAll()).Returns(mockClients);

            var clients = _clientService.GetAll();

            Assert.IsType<List<Client>>(clients);
            Assert.Equal(mockClients[0].Name, clients.ToList()[0].Name);
            Assert.Equal(mockClients[1].Name, clients.ToList()[1].Name);

            _clientRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void ShouldGetClientById()
        {
            var client = new Client("Client 1", "client1@example.com", "12345678901", 123456789);
            _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns(client);

            var foundClient = _clientService.GetById(client.Id);

            Assert.Equal(client.Id, foundClient.Id);
            Assert.Equal(client.Name, foundClient.Name);
            Assert.Equal(client.Email, foundClient.Email);

            _clientRepository.Verify(repo => repo.FindById(client.Id), Times.Once);
        }

        [Fact]
        public void ShouldNotGetClientByUnexistentId()
        {
            var clientId = Guid.NewGuid();
            _clientRepository.Setup(repo => repo.FindById(clientId)).Returns((Client)null);

            Assert.Throws<ClientNotFoundException>(() => _clientService.GetById(clientId));

            _clientRepository.Verify(repo => repo.FindById(clientId), Times.Once);
        }

        [Fact]
        public void ShouldCreateClient()
        {
            var client = new Client("New Client", "newclient@example.com", "11223344556", 123456789);
            _clientRepository.Setup(repo => repo.Create(client)).Returns(client);

            var newClient = _clientService.Create(client);

            Assert.Equal(client.Name, newClient.Name);
            Assert.Equal(client.Email, newClient.Email);

            _clientRepository.Verify(repo => repo.Create(client), Times.Once);
        }

        [Fact]
        public void ShouldUpdateClient()
        {
            var client = new Client("Client", "client@example.com", "12345678901", 123456789);
            var updatedClient = new Client("Updated Client", "updated@example.com", "12345678901", 123456789);

            _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns(client);
            _clientRepository.Setup(repo => repo.Update(client));

            _clientService.Update(client.Id, updatedClient);

            Assert.Equal(client.Name, updatedClient.Name);
            Assert.Equal(client.Email, updatedClient.Email);

            _clientRepository.Verify(repo => repo.FindById(client.Id), Times.Once);
            _clientRepository.Verify(repo => repo.Update(client), Times.Once);
        }

        [Fact]
        public void ShouldNotUpdateClientWithUnexistentId()
        {
            var client = new Client("Client", "client@example.com", "12345678901", 123456789);
            var updatedClient = new Client("Updated Client", "updated@example.com", "12345678901", 123456789);

            _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns((Client)null);

            Assert.Throws<ClientNotFoundException>(() => _clientService.Update(client.Id, updatedClient));

            _clientRepository.Verify(repo => repo.FindById(client.Id), Times.Once);
            _clientRepository.Verify(repo => repo.Update(It.IsAny<Client>()), Times.Never);
        }

        [Fact]
        public void ShouldDeleteClient()
        {
            var client = new Client("Client", "client@example.com", "12345678901", 123456789);

            _clientRepository.Setup(repo => repo.FindById(client.Id)).Returns(client);
            _clientRepository.Setup(repo => repo.Remove(client));

            _clientService.Remove(client.Id);

            _clientRepository.Verify(repo => repo.FindById(client.Id), Times.Once);
            _clientRepository.Verify(repo => repo.Remove(client), Times.Once);
        }

        [Fact]
        public void ShouldNotDeleteClientWithUnexistentId()
        {
            var clientId = Guid.NewGuid();

            _clientRepository.Setup(repo => repo.FindById(clientId)).Returns((Client)null);

            Assert.Throws<ClientNotFoundException>(() => _clientService.Remove(clientId));

            _clientRepository.Verify(repo => repo.FindById(clientId), Times.Once);
            _clientRepository.Verify(repo => repo.Remove(It.IsAny<Client>()), Times.Never);
        }  
  }
}