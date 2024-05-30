namespace Entities
{
  public class Client(string Name, string Cpf, string Email, int Phone, DateTime BirthDate)
  {
    public Guid Id { get; set; } = new();
    public string Name { get; set; } = Name;
    public string Cpf { get; set; } = Cpf;
    public string Email { get; set; } = Email;
    public int Phone { get; set; } = Phone;
    public DateTime BirthDate { get; set; } = BirthDate;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
  }
}