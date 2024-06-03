using Moq;
using Repositories;
using Services;
using Entities;
using Exceptions;

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
    public async Task CreateClient_ValidClient_ReturnsCreatedClient()
    {
      var client = new Client("João", "691.635.930-76", "João@email.com", 1234567890, new DateTime(1985, 10, 15));

      _clientRepository.Setup(repo => repo.FindByCpf(It.IsAny<string>())).Returns((Client)null);
      _clientRepository.Setup(repo => repo.FindByEmail(It.IsAny<string>())).Returns((Client)null);
      _clientRepository.Setup(repo => repo.FindByPhone(It.IsAny<int>())).Returns((Client)null);
      _clientRepository.Setup(repo => repo.Create(It.IsAny<Client>())).Returns(client);

      var result = _clientService.Create(client);

      Assert.Equal(client, result);
    }

    [Fact]
    public async Task CreateClient_DuplicateCpf_ThrowsClientCpfAlreadyRegisteredException()
    {
      var client = new Client("Roberto", "691.635.930-76", "Roberto@email.com", 1234567890, new DateTime(1985, 10, 15));

      _clientRepository.Setup(repo => repo.FindByCpf(It.IsAny<string>())).Returns(client);

      Assert.Throws<ClientCpfAlreadyRegisteredException>(() => _clientService.Create(client));
    }

    [Fact]
    public void CreateClient_InvalidCpf_ThrowsClientInvalidCpfException()
    {
      var client = new Client("João", "123.123.123-12", "joao@example.com", 1234567890, new DateTime(1985, 10, 15));

      Assert.Throws<ClientInvalidCpfException>(() => _clientService.Create(client));
    }

    [Fact]
    public void CreateClient_EmailAlreadyRegistered_ThrowsClientEmailAlreadyRegisteredException()
    {
      var client = new Client("João", "691.635.930-76", "joao@example.com", 1234567890, new DateTime(1985, 10, 15));
      _clientRepository.Setup(repo => repo.FindByEmail(client.Email)).Returns(client);

      Assert.Throws<ClientEmailAlreadyRegisteredException>(() => _clientService.Create(client));
    }

    [Fact]
    public void CreateClient_PhoneAlreadyRegistered_ThrowsClientPhoneAlreadyRegisteredException()
    {
      var client = new Client("João", "691.635.930-76", "joao@example.com", 1234567890, new DateTime(1985, 10, 15));
      _clientRepository.Setup(repo => repo.FindByPhone(client.Phone)).Returns(client);

      Assert.Throws<ClientPhoneAlreadyRegisteredException>(() => _clientService.Create(client));
    }

    [Fact]
    public void CreateClient_InvalidBirthDate_ThrowsClientDateNotAcceptedException()
    {
      var client = new Client("João", "691.635.930-76", "joao@example.com", 1234567890, DateTime.Now.AddYears(1));

      Assert.Throws<ClientDateNotAceptedException>(() => _clientService.Create(client));
    }

    [Fact]
    public void CreateClient_AgeExceedsLimit_ThrowsClientAgeExceededException()
    {
      var client = new Client("João", "691.635.930-76", "joao@example.com", 1234567890, DateTime.Now.AddYears(-121));

      Assert.Throws<ClientAgeExceededException>(() => _clientService.Create(client));
    }

    [Fact]
    public void UpdateClient_ValidClient_UpdatesSuccessfully()
    {
      var clientId = Guid.NewGuid();
      var existingClient = new Client("João", "193.911.100-53", "joao@email.com", 1234567890, new DateTime(1985, 10, 15));
      var updatedClient = new UpdateClientDTO("Camila", "211.141.810-21", "camila@email.com", 987654321, new DateTime(1990, 5, 20));

      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns(existingClient);
      _clientRepository.Setup(repo => repo.FindByCpf(updatedClient.Cpf)).Returns((Client)null);
      _clientRepository.Setup(repo => repo.FindByEmail(updatedClient.Email)).Returns((Client)null);
      _clientRepository.Setup(repo => repo.FindByPhone(updatedClient.Phone)).Returns((Client)null);

      _clientService.Update(clientId, updatedClient);

      _clientRepository.Verify(repo => repo.Update(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public void UpdateClient_ClientNotFound_ThrowsClientNotFoundException()
    {
      var clientId = Guid.NewGuid();
      var updatedClient = new UpdateClientDTO("Camila", "193.261.040-52", "Camila@email.com", 987654321, new DateTime(1990, 5, 20));

      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns((Client)null);

      Assert.Throws<ClientNotFoundException>(() => _clientService.Update(clientId, updatedClient));
    }

    [Fact]
    public void UpdateClient_CpfAlreadyRegistered_ThrowsClientCpfAlreadyExistsException()
    {
      var clientId = Guid.NewGuid();
      var existingClient = new Client("João", "211.141.810-21", "joao@email.com", 1234567890, new DateTime(1985, 10, 15));
      var anotherClient = new Client("Pedro", "211.141.810-21", "pedro@email.com", 555555555, new DateTime(1980, 1, 1));
      var updatedClient = new UpdateClientDTO("Camila", "211.141.810-21", "camila@email.com", 987654321, new DateTime(1990, 5, 20));

      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns(existingClient);
      _clientRepository.Setup(repo => repo.FindByCpf(updatedClient.Cpf)).Returns(anotherClient);

      Assert.Throws<ClientCpfAlreadyExistsException>(() => _clientService.Update(clientId, updatedClient));
    }

    [Fact]
    public void UpdateClient_EmailAlreadyRegistered_ThrowsClientEmailAlreadyExistsException()
    {
      var clientId = Guid.NewGuid();
      var existingClient = new Client("João", "193.261.040-52", "joao.doe@email.com", 1234567890, new DateTime(1985, 10, 15));
      var anotherClient = new Client("Pedro", "090.918.510-74", "pedro@email.com", 555555555, new DateTime(1980, 1, 1));
      var updatedClient = new UpdateClientDTO("Camila", "837.795.820-15", "pedro@email.com", 987654321, new DateTime(1990, 5, 20));

      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns(existingClient);
      _clientRepository.Setup(repo => repo.FindByEmail(updatedClient.Email)).Returns(anotherClient);

      Assert.Throws<ClientEmailAlreadyExistsException>(() => _clientService.Update(clientId, updatedClient));
    }

    [Fact]
    public void UpdateClient_PhoneAlreadyRegistered_ThrowsClientPhoneAlreadyExistsException()
    {
      var clientId = Guid.NewGuid();
      var existingClient = new Client("João", "193.261.040-52", "joao@email.com", 1234567890, new DateTime(1985, 10, 15));
      var anotherClient = new Client("Pedro", "090.918.510-74", "pedro@email.com", 987654321, new DateTime(1980, 1, 1));
      var updatedClient = new UpdateClientDTO("Camila", "837.795.820-15", "camila@email.com", 987654321, new DateTime(1990, 5, 20));

      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns(existingClient);
      _clientRepository.Setup(repo => repo.FindByPhone(updatedClient.Phone)).Returns(anotherClient);

      Assert.Throws<ClientPhoneAlreadyExistsException>(() => _clientService.Update(clientId, updatedClient));
    }

    [Fact]
    public void RemoveClient_ClientExists_RemovesSuccessfully()
    {
      var clientId = Guid.NewGuid();
      var existingClient = new Client("João", "193.261.040-52", "joao@email.com", 1234567890, new DateTime(1985, 10, 15));
      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns(existingClient);

      _clientService.Remove(clientId);

      _clientRepository.Verify(repo => repo.Remove(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public void RemoveClient_ClientDoesNotExist_ThrowsClientNotFoundException()
    {
      var clientId = Guid.NewGuid();
      _clientRepository.Setup(repo => repo.FindById(clientId)).Returns((Client)null);

      Assert.Throws<ClientNotFoundException>(() => _clientService.Remove(clientId));
    }
  }
}