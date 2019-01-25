using System;

namespace PaymentGateway
{
    public class PaymentGatewaySettings
    {
        public Uri BankApiUri { get; set; }
        public Uri RegisterOrderRoute { get; set; }
        public Uri PaymentRoute { get; set; }
        public Uri CancellationRoute { get; set; }
        public Uri ReturnSuccessUrl { get; set; }
        public Uri ReturnFailedUrl { get; set; }
    }
}