using System;

namespace PaymentGateway
{
    public class PaymentGatewaySettings
    {
        public Uri BankApiUri { get; set; }
        public Uri SinglePaymentRouteUri { get; set; }
        public Uri RecurrentPaymentRouteUri { get; set; }
        public Uri ReturnUrl { get; set; }
    }
}