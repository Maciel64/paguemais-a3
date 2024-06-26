using Config;
using Entities;

namespace Repositories
{
  // Design Pattern | Abstract Factory
  public interface IProductRepository
  {
    public IEnumerable<Product> GetAll();
    Product? FindById(Guid productId);
    public Product Create(Product product);

    void Update(Product product);
    public void Remove(Product product);
  }

  public class ProductRepository(Database context) : IProductRepository
  {
    public readonly Database _context = context;

    //Método para retornar todos os Produtos
    public IEnumerable<Product> GetAll()
    {
      return [.. _context.Products];
    }

    //Método para criar Produtos
    public Product Create(Product product)
    {
      _context.Products.Add(product);
      _context.SaveChangesAsync();

      return product;
    }

    //Método para achar Produto pelo ID
    public Product? FindById(Guid id)
    {
      return _context.Products.Find(id);
    }

    //Método para Remover o Produto
    public void Remove(Product product)
    {
      _context.Products.Remove(product);
      _context.SaveChanges();
    }

    //Método para Editar Producte
    public void Update(Product product)
    {
      _context.Products.Update(product);
      _context.SaveChanges();
    }
  }
}