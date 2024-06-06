using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class ProductService(IProductRepository productRepository, ICartRepository cartRepository)
  {
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICartRepository _cartRepository = cartRepository;


    public IEnumerable<Product> GetAll()
    {
      return _productRepository.GetAll();
    }

    //Método para achar o Produto pelo ID
    public Product GetById(Guid productId)
    {
      var product = _productRepository.FindById(productId) ?? throw new ProductNotFoundException();
      return product;
    }

    public Product Create(Product product)
    {
      if (product.Price < 0)
      {
        throw new ProductPriceIsInvalidException();
      }

      return _productRepository.Create(product);
    }

    //Remover Produto//
    public void Remove(Guid productId)
    {
      var product = _productRepository.FindById(productId) ?? throw new ProductNotFoundException();
      var cart = _cartRepository.FindByProductId(productId);

      if (cart is not null)
      {
        throw new ProductInUseExeption();
      }

      _productRepository.Remove(product);
    }

    //Editar Produto//
    public Product Update(Guid productId, UpdateProductDTO updatedProduct)
    {
      var product = _productRepository.FindById(productId) ?? throw new ProductNotFoundException();

      if (updatedProduct.Price is not null)
      {
        if (updatedProduct.Price < 0)
        {
          throw new ProductPriceIsInvalidException();
        }

        product.Price = (float)updatedProduct.Price;
      }

      if (updatedProduct.Name is not null)
      {
        product.Name = updatedProduct.Name;
      }

      _productRepository.Update(product);
      return product;
    }
  }
}