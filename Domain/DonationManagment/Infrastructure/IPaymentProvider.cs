using System.Threading.Tasks;

namespace DonationManagment.Infrastructure
{
    public interface IPaymentProvider
    {
        DonationPaymentInformation InitiateSinglePaymentForDonation(Donation donation);
        DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation);
    }
}