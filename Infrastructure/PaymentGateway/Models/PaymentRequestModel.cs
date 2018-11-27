namespace PaymentGateway.Models
{
    public class PaymentRequestModel
    {
        public int OrderNumber { get; set; }
        public int Amount { get; set; }
        public string ReturnUrl { get; set; }
    }
}