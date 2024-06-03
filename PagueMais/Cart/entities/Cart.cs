using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
  public class Cart(Guid ProductId, Guid PurchaseId, int Quantity)
  {
    public Guid Id { get; init; } = new();
    public int Quantity { get; set; } = Quantity;

    [ForeignKey("Product")]
    public Guid ProductId { get; set; } = ProductId;

    public Product? Product { get; set; }



    [ForeignKey("Purchase")]
    public Guid PurchaseId { get; set; } = PurchaseId;

    public Purchase? Purchase { get; set; }
  }
}