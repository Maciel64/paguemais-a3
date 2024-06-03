using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public interface IClientRepository
  {
    public IEnumerable<Client> GetAll();
    public Client Create(Client client);
    public Client? FindById(Guid id);
    public Client? FindByCpf(string cpf);
    public Client? FindByEmail(string email);
    public Client? FindByPhone(int phone);
    public void Update(Client client);
    public void Remove(Client client);
  }

  public class ClientRepository(Database context) : IClientRepository
  {
    private readonly Database _context = context;

    //Método para printar todos os Clientes
    public IEnumerable<Client> GetAll()
    {
      return [.. _context.Clients];
    }

    //Método para criar Clientes
    public Client Create(Client client)
    {
      _context.Clients.Add(client);
      _context.SaveChanges();

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
    public void Remove(Client client)
    {
      _context.Clients.Remove(client);
      _context.SaveChanges();
    }

    //Método para achar Cliente pelo CPF
    public Client? FindByCpf(string cpf)
    {
      return _context.Clients.FirstOrDefault(client => client.Cpf == cpf);
    }

    //Método para achar Cliente pelo Email
    public Client? FindByEmail(string email)
    {
      return _context.Clients.FirstOrDefault(client => client.Email == email);
    }

    //Método para achar Cliente pelo Telefone
    public Client? FindByPhone(int phone)
    {
      return _context.Clients.FirstOrDefault(client => client.Phone == phone);
    }

    //Método para Editar Cliente
    public void Update(Client client)
    {
      _context.Clients.Update(client);
      _context.SaveChanges();
    }
  }
}