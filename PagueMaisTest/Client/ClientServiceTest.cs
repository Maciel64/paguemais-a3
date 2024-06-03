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
  }
}