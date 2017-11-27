using System.Threading.Tasks;
using CapitalManagment;
using UserManagment;

namespace DonationManagment.Infrastructure
{
    public interface IPaymentProvider
    {
        DonationPaymentInformation InitiateSinglePaymentForDonation(Donation donation, CapitalCredentials credentials, User fromUser);
        DonationPaymentInformation InitiateRequrrentPaymentForDonation(Donation donation, CapitalCredentials credentials, User fromUser);
    }
}