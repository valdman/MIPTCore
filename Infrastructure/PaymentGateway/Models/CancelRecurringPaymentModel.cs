namespace PaymentGateway.Models
{
    public class CancelRecurringPaymentModel
    {
        public string Sector { get; set; }
        public string Id { get; set; }
        public string Password { private get; set; }
        
        public string Signature => SignatureHelper.SignVia(Sector, Id, Password);
    }
}