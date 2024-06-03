using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public interface IPurchaseRepository
  {
    public IEnumerable<Purchase> GetAll();
    public Purchase Create(Purchase purchase);
    public Purchase? FindById(Guid id);
    public void Remove(Purchase purchase);
    public void Update(Purchase purchase);
  }

  public class PurchaseRepository(Database context) : IPurchaseRepository
  {
    private readonly Database _context = context;

    //Método para printar todos as compra
    public IEnumerable<Purchase> GetAll()
    {
      return [.. _context.Purchases];
    }

    //Método para criar uma compra
    public Purchase Create(Purchase purchase)
    {
      _context.Purchases.Add(purchase);
      _context.SaveChanges();

      return purchase;
    }

    //Método para achar Compra pelo ID
    public Purchase? FindById(Guid id)
    {
      return _context.Purchases
        .Include(p => p.Client)
        .Include(p => p.Carts)
        .FirstOrDefault(p => p.Id == id);
    }

    //Método para Remover a Compra
    public void Remove(Purchase purchase)
    {
      _context.Purchases.Remove(purchase);
      _context.SaveChanges();
    }

    //Método para Editar Compra
    public void Update(Purchase purchase)
    {
      _context.Purchases.Update(purchase);
      _context.SaveChanges();
    }
  }
}