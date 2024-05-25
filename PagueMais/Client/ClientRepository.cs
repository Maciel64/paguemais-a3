using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public class ClientRepository(Database context)
  {
    private readonly Database _context = context;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
      return await _context.Clients.ToListAsync();
    }

    public async Task<Client> CreateAsync(Client client)
    {
      _context.Clients.Add(client);
      await _context.SaveChangesAsync();

      return client;
    }

    public async Task<Client?> FindByCpf(string Cpf)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Cpf == Cpf);
    }
  }
}