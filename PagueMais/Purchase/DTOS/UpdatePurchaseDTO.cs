namespace Entities
{
    public class UpdatePurchaseDTO
    {
        public float? Total { get; set; }
        public EnumStatus? Status { get; set; }
        public EnumMethods? PaymentMethod { get; set; }
        public DateTime? FinishedAtl { get; set; }
    }
}