using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Exceptions;

namespace Controllers
{
  [ApiController]
  [Route("/api/v1/carts")]
  public class CartController : ControllerBase
  {
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
      _cartService = cartService;
    }

    //Criar um novo Cart
    [HttpPost]
    public ActionResult Create([FromBody] Cart cart)
    {
      try
      {
        var createdCart = _cartService.Create(cart.ProductId, cart.PurchaseId);
        return Ok(createdCart);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //Aumentar a quantidade do produto
    [HttpGet]
    [Route("increment/{Id}")]
    public ActionResult PlusProductQuantity(Guid Id)
    {
      try
      {
        _cartService.IncrementProductQuantity(Id);
        return Ok();
      }
      catch (CartNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //Diminuir quantidado do produto
    [HttpGet]
    [Route("decrement/{Id}")]
    public ActionResult MinusProductQuantity(Guid Id)
    {
      try
      {
        _cartService.DecrementProductQuantity(Id);
        return Ok();
      }
      catch (CartNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (CartQuantityIsZeroException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }


    //Remover produto
    [HttpDelete]
    [Route("remove/{Id}")]
    public ActionResult RemoveProductFromCart(Guid Id)
    {
      try
      {
        _cartService.RemoveProductFromCart(Id);
        return Ok();
      }
      catch (CartNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //Printar todos os Carts
    [HttpGet]
    public ActionResult<IEnumerable<Cart>> GetAllCarts()
    {
      try
      {
        var carts = _cartService.GetAllCarts();
        return Ok(carts);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
