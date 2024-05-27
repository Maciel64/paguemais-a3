namespace Entities
{
  public class Client(string Name, string Cpf, string Email, int Phone, DateTime BirthDate)
  {
    // Propriedade que armazena o ID do cliente
    public Guid Id { get; set; } = new();

    // Propriedade que armazena o nome do cliente
    public string Name { get; set; } = Name;

    // Propriedade que armazena o CPF do cliente
    public string Cpf { get; set; } = Cpf;

    // Propriedade que armazena o email do cliente
    public string Email { get; set; } = Email;

    // Propriedade que armazena o telefone do cliente
    public int Phone { get; set; } = Phone;

    // Propriedade que armazena a data de nascimento do cliente
    public DateTime BirthDate { get; set; } = BirthDate;

    // Propriedade que armazena a data de criação do cliente
    public DateTime CreatedAt { get; set; } = new();

    // Propriedade que armazena a data de atualização do cliente
    public Nullable<DateTime> UpdatedAt { get; set; } = null;
  }
}