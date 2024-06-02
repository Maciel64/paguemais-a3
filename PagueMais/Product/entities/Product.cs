namespace Entities
{
  public class Product(string Name, float Price)
  {
    public Guid Id { get; init; } = new();
    public string Name { get; set; } = Name;
    public float Price { get; set; } = Price;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
  }
}