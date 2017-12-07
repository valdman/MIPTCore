using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DonationManagment.Application
{
    public interface IDonationManager
    {
        IEnumerable<Donation> GetAllDonations();
        IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null);
        Donation GetDonationByIdAsync(int donationId);
        
        DonationPaymentInformation CreateDonationAsync(Donation donationToCreate);
        int CreateCompletedSingleDonation(Donation donationToCreate);
        void ConfirmDonation(Donation donationToConfirm);

        void UpdateDonationAsync(Donation donationToUpdate);
        void DeleteDonation(int donationToDeleteId);
    }
}