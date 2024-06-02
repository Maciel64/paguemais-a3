using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum EnumStatus
{
    Pending,
    Completed,
    Canceled
}

public enum EnumMethods
{
    Credit,
    Debit,
    Pix,  
    Money
}

namespace Entities
{
    public class Purchase(int Total, EnumStatus Status, DateTime FinishedAt, EnumMethods PaymentMethod)
    {
        // Propriedades - Construtor
        [Key]
        public Guid Id { get; set; } = new();
        public float Total { get; set; } = Total;
        public EnumStatus Status { get; set; } = Status;
        public DateTime FinishedAt { get; set; } = FinishedAt;
        public EnumMethods PaymentMethod { get; set; } = PaymentMethod;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        [ForeignKey("Client")]
        public Guid ClientId {get; set;} 
        public Client Client {get; set;} 
    }
}