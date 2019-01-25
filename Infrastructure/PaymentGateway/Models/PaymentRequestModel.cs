namespace PaymentGateway.Models
{
    public class PaymentRequestModel
    {
        public string Sector { get; set; }
        public string Password { private get; set; }
        
        public int Amount { get; set; }
        public int Currency { get; set; }
        public int Reference { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Failurl { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RecurringPeriod { get; set; }

        public string Signature => SignatureHelper.SignVia(Sector, Amount, Currency, Password);
    }
}