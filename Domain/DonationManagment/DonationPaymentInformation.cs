namespace DonationManagment
{
    public class DonationPaymentInformation
    {
        public DonationPaymentInformation(int donationId, string paymentUri, string orderId)
        {
            DonationId = donationId;
            PaymentUri = paymentUri;
            OrderId = orderId;
        }

        public int DonationId { get; private set; }
        public string OrderId { get; set; }
        public string PaymentUri { get; private set; }
    }
}