using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public class ClientRepository(Database context)
  {
    private readonly Database _context = context;

    //Método para printar todos os Clientes
    public async Task<IEnumerable<Client>> GetAllAsync()
    {
      return await _context.Clients.ToListAsync();
    }

    //Método para criar Clientes
    public async Task<Client> CreateAsync(Client client)
    {
      _context.Clients.Add(client);
      await _context.SaveChangesAsync();

      return client;
    }

    //Método para achar Cliente pelo ID
    public Client? FindById(Guid id)
    {
      return _context.Clients
        .Include(c => c.Purchases)
        .FirstOrDefault(c => c.Id == id);
    }

    //Método para Remover o Cliente
    public async Task RemoveAsync(Client client)
    {
      _context.Clients.Remove(client);
      await _context.SaveChangesAsync();
    }

    //Método para achar Cliente pelo CPF
    public async Task<Client?> FindByCpfAsync(string cpf)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Cpf == cpf);
    }

    //Método para achar Cliente pelo Email
    public async Task<Client?> FindByEmailAsync(string email)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Email == email);
    }

    //Método para achar Cliente pelo Telefone
    public async Task<Client?> FindByPhoneAsync(int phone)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Phone == phone);
    }

    //Método para Editar Cliente
    public async Task UpdateAsync(Client client)
    {
      _context.Clients.Update(client);
      await _context.SaveChangesAsync();
    }

    //Método para achar Cliente pelo CPF
    public Client? FindByCpf(string Cpf)
    {
      return _context.Clients.FirstOrDefault(client => client.Cpf == Cpf);
    }

    //Método para achar Cliente pelo Email
    public async Task<Client?> FindByEmail(string Email)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Email == Email);
    }

    //Método para achar Cliente pelo Phone
    public async Task<Client?> FindByPhone(int Phone)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Phone == Phone);
    }

  }
}