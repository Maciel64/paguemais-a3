using Entities;
using Exceptions;
using Moq;
using Repositories;
using Services;

namespace Tests
{
  public class ProductServiceTest
  {
    private readonly Mock<IProductRepository> _productRepository;
    private readonly ProductService _productService;

    public ProductServiceTest()
    {
      _productRepository = new Mock<IProductRepository>();
      _productService = new ProductService(_productRepository.Object);
    }


    [Fact]
    public void ShouldGetAllProducts()
    {
      var mockProducts = new List<Product> {
        new("Produto 1", 10),
        new("Produto 2", 23)
      };

      _productRepository.Setup(repo => repo.GetAll()).Returns(mockProducts);

      var products = _productService.GetAll();

      Assert.IsType<List<Product>>(products);
      Assert.Equal(mockProducts[0].Name, products.ToList()[0].Name);
      Assert.Equal(mockProducts[1].Name, products.ToList()[1].Name);
      Assert.Equal(mockProducts[0].Price, products.ToList()[0].Price);
      Assert.Equal(mockProducts[1].Price, products.ToList()[1].Price);

      _productRepository.Verify(repo => repo.GetAll(), Times.Once);
    }


    [Fact]
    public void ShouldGetOneProductById()
    {
      var product = new Product("Mock", 10);
      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns(product);

      var foundProduct = _productService.GetById(product.Id);

      Assert.Equal(product.Id, foundProduct.Id);
      Assert.Equal(product.Name, foundProduct.Name);
      Assert.Equal(product.Price, foundProduct.Price);

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
    }


    [Fact]
    public void ShouldNotGetOneProductByUnexistentId()
    {
      var product = new Product("Mock", 10);

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns((Guid id) => null);

      Assert.Throws<ProductNotFoundException>(() => _productService.GetById(product.Id));

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
    }


    [Fact]
    public void ShouldBeAbleToCreateProduct()
    {
      var product = new Product("Test Product", 0);

      _productRepository.Setup(repo => repo.Create(product)).Returns(product);

      var newProduct = _productService.Create(product);

      Assert.Equal("Test Product", newProduct.Name);
      Assert.Equal(0, newProduct.Price);

      _productRepository.Verify(repo => repo.Create(product), Times.Once);
    }


    [Theory]
    [InlineData(-10)]
    public void ShouldNotBeAbleToCreateProductWithInvalidData(float Price)
    {
      var product = new Product("Test Product", Price);

      Assert.Throws<ProductPriceIsInvalidException>(() => _productService.Create(product));

      _productRepository.Verify(repo => repo.Create(product), Times.Never);
    }

    [Fact]
    public void ShouldBeAbleToUpdateProduct()
    {
      var product = new Product("Test Product", 10);

      UpdateProductDTO dto = new()
      {
        Name = "Novo nome",
        Price = 12
      };

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns(product);
      _productRepository.Setup(repo => repo.Update(product));

      var updatedProduct = _productService.Update(product.Id, dto);

      Assert.Equal(updatedProduct.Name, dto.Name);
      Assert.Equal(updatedProduct.Price, dto.Price);

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
      _productRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void ShouldNotBeAbleToUpdateProductToNegativePrice()
    {
      var product = new Product("Test Product", 10);

      UpdateProductDTO dto = new()
      {
        Name = "Novo nome",
        Price = -1
      };

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns(product);
      _productRepository.Setup(repo => repo.Update(product));

      Assert.Throws<ProductPriceIsInvalidException>(() => _productService.Update(product.Id, dto));

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
      _productRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void ShouldNotBeAbleToUpdateProductWithInexistentId()
    {
      var product = new Product("Test Product", 10);

      UpdateProductDTO dto = new()
      {
        Name = "Novo nome",
        Price = 10
      };

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns((Guid Id) => null);
      _productRepository.Setup(repo => repo.Update(product));


      Assert.Throws<ProductNotFoundException>(() => _productService.Update(product.Id, dto));

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
      _productRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void ShouldBeAbleToDeleteProduct()
    {
      var product = new Product("Test Product", 10);

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns(product);
      _productRepository.Setup(repo => repo.Remove(product));

      _productService.Remove(product.Id);

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
      _productRepository.Verify(repo => repo.Remove(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void ShouldNotBeAbleToDeleteProductWithInexistentId()
    {
      var product = new Product("Test Product", 10);

      _productRepository.Setup(repo => repo.FindById(product.Id)).Returns((Guid Id) => null);
      _productRepository.Setup(repo => repo.Remove(product));

      Assert.Throws<ProductNotFoundException>(() => _productService.Remove(product.Id));

      _productRepository.Verify(repo => repo.FindById(product.Id), Times.Once);
      _productRepository.Verify(repo => repo.Remove(product), Times.Never);
    }
  }
}
