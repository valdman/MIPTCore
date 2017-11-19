using System;

namespace PaymentGateway
{
    public class PaymentGatewaySettings
    {
        public string MerchantLogin { get; set; }
        public string MerchantPassword { get; set; }
        
        public Uri BankApiUri { get; set; }
        public Uri SinglePaymentRouteUri { get; set; }
        public Uri RecurrentPaymentRouteUri { get; set; }
        public Uri ReturnUrl { get; set; }
    }
}