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
        public DateTime CreatedAt { get; set; } = new();
        public Nullable<DateTime> UpdatedAt { get; set; } = null;

        public static implicit operator Client(bool v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator bool(Client v)
        {
            throw new NotImplementedException();
        }
    }
}