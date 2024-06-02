namespace Entities
{
    public class UpdatePurchaseDTO(float Total, string Status, DateTime FinishedAt, string PaymentMethod)
    {
        public float? Total { get; set; } = Total;
        public string? Status { get; set; } = Status;
        public DateTime? FinishedAtl { get; set; } = FinishedAt;
        public string? PaymentMethod { get; set; } = PaymentMethod;
    }
}