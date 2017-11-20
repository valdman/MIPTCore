namespace PaymentGateway.Models
{
    public class ParametrizedPaymentRequestModel : PaymentRequestModel
    {
        public int ClientId { get; set; }
        public string JsonParams { get; set; }
    }
}