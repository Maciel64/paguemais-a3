using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
  [ApiController]
  [Route("/api/v1/purchases")]
  public class PurchaseController(PurchaseService purchaseService) : ControllerBase
  {
    private readonly PurchaseService _purchaseService = purchaseService;

    //Método para printar todos os es
    [HttpGet]
    public ActionResult<IEnumerable<Purchase>> GetAll()
    {
      var produtos = _purchaseService.GetAll();
      return Ok(produtos);
    }

    //Método para criar um novo e
    [HttpPost]
    public ActionResult<IEnumerable<Purchase>> Create([FromBody] Purchase purchase)
    {
      //Cria novo e
      _purchaseService.Create(purchase);
      return Ok(purchase);

    }

    //Método para deletar e
    [HttpDelete("{Id}")]
    public IActionResult Delete(Guid id)
    {
      try
      {
        //Deleta o e
        _purchaseService.Remove(id);
        return NoContent();
      }

      //Retorno de ERRO caso ID do e não seja encontrado
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    //Método para Editar e
    [HttpPut("{Id}")]
    public IActionResult Update(Guid id, [FromBody] UpdatePurchaseDTO purchase)
    {
      try
      {
        //Atualiza o e
        _purchaseService.Update(id, purchase);

        //Busca e atualizado
        var updatedPurchase = _purchaseService.GetPurchaseById(id);

        //Retorna e atualizado
        return Ok(updatedPurchase);
      }

      //Retorno de ERRO caso e não exista
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    //Método para printar e pelo ID
    [HttpGet("{Id}")]
    public IActionResult GetById(Guid id)
    {
      try
      {

        //Busca e
        var purchase = _purchaseService.GetPurchaseById(id);

        //Retorna es
        return Ok(purchase);
      }

      //Retorno de ERRO caso e não exista
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
