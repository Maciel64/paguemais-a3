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
      var registeredClientCpf = await _produtoRepository.FindByCpf(client.Cpf);

      if (registeredClientCpf is not null)
      {
        throw new ClientCpfAlreadyRegisteredException();
      }
      var registeredClientEmail = await _produtoRepository.FindByEmail(client.Email);

      if (registeredClientEmail is not null)
      {
        throw new ClientCpfAlreadyRegisteredException();
      }
      var registeredClientPhone = await _produtoRepository.FindByPhone(client.Phone);

      if (registeredClientPhone is not null)
      {
        throw new ClientCpfAlreadyRegisteredException();
      }



      return await _produtoRepository.CreateAsync(client);
    }
  }
}