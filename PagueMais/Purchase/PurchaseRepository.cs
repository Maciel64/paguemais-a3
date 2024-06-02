using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public class PurchaseRepository(Database context)
  {
    private readonly Database _context = context;

    //Método para printar todos as compra
    public async Task<IEnumerable<Purchase>> GetAllAsync()
    {
      return await _context.Purchase.ToListAsync();
    }

    //Método para criar uma compra
    public async Task<Purchase> CreateAsync(Purchase purchase)
    {
      _context.Purchase.Add(purchase);
      await _context.SaveChangesAsync();

      return purchase;
    }

    //Método para achar Compra pelo ID
    public async Task<Purchase?> FindByIdAsync(Guid id)
    {
      return await _context.Purchase.FindAsync(id);
    }

    //Método para Remover a Compra
    public async Task RemoveAsync(Purchase purchase)
    {
      _context.Purchase.Remove(purchase);
      await _context.SaveChangesAsync();
    }

    //Método para Editar Compra
    public async Task UpdateAsync(Purchase purchase)
    {
      _context.Purchase.Update(purchase);
      await _context.SaveChangesAsync();
    }

  }
}