namespace Entities
{
    public class UpdateClientDTO(string Name, string Cpf, string Email, int Phone, DateTime BirthDate)
    {
        public string Name { get; set; } = Name;
        public string Cpf { get; set; } = Cpf;
        public string Email { get; set; } = Email;
        public int Phone { get; set; } = Phone;
        public DateTime BirthDate { get; set; } = BirthDate;
    }
}