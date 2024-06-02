using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities
{
  public class Client(string Name, string Cpf, string Email, int Phone, DateTime BirthDate)
  {
    [Key]
    public Guid Id { get; set; } = new();
    public string Name { get; set; } = Name;
    public string Cpf { get; set; } = Cpf;
    public string Email { get; set; } = Email;
    public int Phone { get; set; } = Phone;
    public DateTime BirthDate { get; set; } = BirthDate;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    [JsonIgnore]
    public ICollection<Purchase>? Purchases { get; set; }
  }
}