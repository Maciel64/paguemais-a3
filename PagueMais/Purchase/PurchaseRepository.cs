using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public class PurchaseRepository(Database context)
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
      return _context.Purchases.Find(id);
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