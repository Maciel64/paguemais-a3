using Config;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
  public interface ICartRepository
  {
    public void Create(Cart cart);
    public void Update(Cart cart);
    public void Remove(Cart cart);
    public void RemoveAll(IEnumerable<Cart> carts);
    public Cart? FindById(Guid Id);
    public Cart? FindByProductId(Guid ProductId);
    public Cart? FindByProductAndPurchaseId(Guid ProductId, Guid PurchaseId);
    public IEnumerable<Cart> GetAllCarts();
  }

  public class CartRepository : ICartRepository
  {
    public Database _context = new();

    //Criar Cart
    public void Create(Cart cart)
    {
      _context.Carts.Add(cart);
      _context.SaveChanges();
    }

    //Editar Cart
    public void Update(Cart cart)
    {
      _context.Carts.Update(cart);
      _context.SaveChanges();
    }

    public void Remove(Cart cart)
    {
      _context.Carts.Remove(cart);
      _context.SaveChanges();
    }

    public void RemoveAll(IEnumerable<Cart> carts)
    {
      _context.Carts.RemoveRange(carts);
      _context.SaveChanges();
    }

    //Achar Cart por ID
    public Cart? FindById(Guid Id)
    {
      return _context.Carts.Find(Id);
    }

    //Achar Produto por ID
    public Cart? FindByProductId(Guid ProductId)
    {
      return _context.Carts.FirstOrDefault(cart => cart.ProductId == ProductId);
    }

    //Achar Produto e Compra por ID
    public Cart? FindByProductAndPurchaseId(Guid ProductId, Guid PurchaseId)
    {
      return _context.Carts.FirstOrDefault(cart => cart.ProductId == ProductId && cart.PurchaseId == PurchaseId);
    }

    //Pegar todos os Carts
    public IEnumerable<Cart> GetAllCarts()
    {
      return _context.Carts.ToList();
    }
  }
}
