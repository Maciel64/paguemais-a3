using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
  [ApiController]
  [Route("api/v1/products")]
  public class ProductController(ProductService productService) : ControllerBase
  {

    private readonly ProductService _productService = productService;


    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
      var produtos = _productService.GetAll();
      return Ok(produtos);
    }

    //Método para criar um novo produto
    [HttpPost]
    public ActionResult<IEnumerable<Product>> Create([FromBody] Product product)
    {
      try
      {
        //Cria novo Cliente
        _productService.Create(product);
        return Ok(product);
      }

      //Retorno de ERRO caso CPF já exista
      catch (Exception e)
      {
        return UnprocessableEntity(e.Message);
      }
    }

    //Método para printar Cliente pelo ID
    [HttpGet("{Id}")]
    public IActionResult GetById(Guid id)
    {
      try
      {

        //Busca Cliente
        var product = _productService.GetById(id);

        //Retorna Clientes
        return Ok(product);
      }

      //Retorno de ERRO caso Produto não exista
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    //Método para deletar Cliente
    [HttpDelete("{Id}")]
    public IActionResult Delete(Guid id)
    {
      try
      {
        //Deleta o Cliente
        _productService.Remove(id);
        return NoContent();
      }

      //Retorno de ERRO caso ID do Cliente não seja encontrado
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    //Método para Editar Cliente
    [HttpPut("{Id}")]
    public ActionResult<Product> Update(Guid id, [FromBody] UpdateProductDTO product)
    {
      try
      {
        //Atualiza o Cliente
        _productService.Update(id, product);

        //Busca Cliente atualizado
        var updatedClient = _productService.GetById(id);

        //Retorna cliente atualizado
        return Ok(updatedClient);
      }

      //Retorno de ERRO caso Cliente não exista
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}