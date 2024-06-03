namespace Entities
{
    public class UpdateClientDTO
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public DateTime BirthDate { get; set; }

        public UpdateClientDTO() { }

        public UpdateClientDTO(string name, string cpf, string email, int phone, DateTime birthDate)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
            Phone = phone;
            BirthDate = birthDate;
        }
    }
}
