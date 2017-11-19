namespace DonationManagment
{
    public class DonationPaymentInformation
    {
        public DonationPaymentInformation(int donationId, string paymentUri)
        {
            DonationId = donationId;
            PaymentUri = paymentUri;
        }

        public int DonationId { get; private set; }
        public string PaymentUri { get; private set; }
    }
}