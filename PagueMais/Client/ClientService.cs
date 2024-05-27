using System.Runtime.InteropServices;
using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class ClientService(ClientRepository produtoRepository)
  {
    private readonly ClientRepository _produtoRepository = produtoRepository;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
      return await _produtoRepository.GetAllAsync();
    }

    public async Task<Client> CreateAsync(Client client)
    {
      var registeredClient = await _produtoRepository.FindByCpf(client.Cpf);

      if (registeredClient is not null)
      {
        throw new ClientCpfAlreadyRegisteredException();
      }



      return await _produtoRepository.CreateAsync(client);
    }
  }
}