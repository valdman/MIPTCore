namespace PaymentGateway.Models
{
    public class ParametrizedPaymentRequestModel : PaymentRequestModel
    {
        public string JsonParams { get; set; }
    }
}