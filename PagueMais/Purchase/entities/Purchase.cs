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
    public class Purchase
    {
        public Purchase()
        {

        }

        public Purchase(float Total, EnumMethods PaymentMethod, Guid ClientId)
        {
            this.Total = Total;
            this.PaymentMethod = PaymentMethod;
            this.ClientId = ClientId;
        }

        [Key]
        public Guid Id { get; set; } = new();
        public float Total { get; set; }
        public EnumMethods? PaymentMethod { get; set; }
        public EnumStatus Status { get; set; } = EnumStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;

        [ForeignKey("Client")]
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public IEnumerable<Cart>? Carts { get; set; }
    }
}