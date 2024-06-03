using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
  [ApiController]
  [Route("/api/v1/carts")]
  public class CartController(CartService cartService) : ControllerBase
  {


    private readonly CartService _cartService = cartService;

    [HttpGet]
    [Route("increment/{Id}")]
    public ActionResult PlusProductQuantity(Guid Id)
    {
      _cartService.IncrementProductQuantity(Id);

      return Ok();
    }

    [HttpGet]
    [Route("decrement/{Id}")]
    public ActionResult MinusProductQuantity(Guid Id)
    {
      return Ok();
    }

    [HttpPost]
    public ActionResult Create([FromBody] Cart cart)
    {
      var createdCart = _cartService.Create(cart.ProductId, cart.PurchaseId);

      return Ok(createdCart);
    }
  }
}