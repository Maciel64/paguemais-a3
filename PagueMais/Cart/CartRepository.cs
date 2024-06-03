using Config;
using Entities;

namespace Repositories
{
  public interface ICartRepository
  {
    public void Create(Cart cart);
    public void Update(Cart cart);
    public Cart? FindById(Guid Id);
    public Cart? FindByProductId(Guid ProductId);
    public Cart? FindByProductAndPurchaseId(Guid ProductId, Guid PurchaseId);
  }

  public class CartRepository : ICartRepository
  {
    public Database _context = new();

    public void Create(Cart cart)
    {
      _context.Carts.Add(cart);
      _context.SaveChanges();
    }

    public void Update(Cart cart)
    {
      _context.Carts.Update(cart);
      _context.SaveChanges();
    }

    public Cart? FindById(Guid Id)
    {
      return _context.Carts.Find(Id);
    }

    public Cart? FindByProductId(Guid ProductId)
    {
      return _context.Carts.FirstOrDefault(cart => cart.ProductId == ProductId);
    }

    public Cart? FindByProductAndPurchaseId(Guid ProductId, Guid PurchaseId)
    {
      return _context.Carts.FirstOrDefault(cart => cart.ProductId == ProductId && cart.PurchaseId == PurchaseId);
    }
  }
}