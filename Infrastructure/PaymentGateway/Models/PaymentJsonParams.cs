namespace PaymentGateway.Models
{
    public class PaymentJsonParams
    {
        public int? RecurringFrequency { get; set; }
        public int? RecurringExpiry { get; set; }
        public string Email { get; set; }
    }
}