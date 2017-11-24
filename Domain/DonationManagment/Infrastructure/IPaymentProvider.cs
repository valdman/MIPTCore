using System.Threading.Tasks;
using CapitalManagment;

namespace DonationManagment.Infrastructure
{
    public interface IPaymentProvider
    {
        DonationPaymentInformation InitiateSinglePaymentForDonation(Donation donation, CapitalCredentials credentials);
        DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation, CapitalCredentials credentials);
    }
}