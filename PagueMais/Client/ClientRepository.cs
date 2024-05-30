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

    public async Task<Client?> FindByIdAsync(Guid id)
    {
      return await _context.Clients.FindAsync(id);
    }

    public async Task RemoveAsync(Client client)
    {
      _context.Clients.Remove(client);
      await _context.SaveChangesAsync();
    }

    public async Task<Client?> FindByCpfAsync(string cpf)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Cpf == cpf);
    }

    public async Task<Client?> FindByEmailAsync(string email)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Email == email);
    }
    public async Task<Client?> FindByPhoneAsync(int? phone)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Phone == phone);
    }
    public async Task UpdateAsync(Client client)
    {
      _context.Clients.Update(client);
      await _context.SaveChangesAsync();
    }

    public async Task<Client?> FindByCpf(string Cpf)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Cpf == Cpf);
    }
    public async Task<Client?> FindByEmail(string Email)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Email == Email);
    }
    public async Task<Client?> FindByPhone(int Phone)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Phone == Phone);
    }

  }
}