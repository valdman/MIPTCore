using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;

namespace DonationManagment.Application
{
    public interface IDonationManager
    {
        IEnumerable<Donation> GetAllDonations();
        IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate);
        PaginatedList<Donation> GetPaginated(PaginationAndFilteringParams filteringParams, Expression<Func<Donation, bool>> predicate = null);
        Donation GetDonationById(int donationId);
        
        DonationPaymentInformation CreateDonationAsync(Donation donationToCreate);
        int CreateCompletedSingleDonation(Donation donationToCreate);
        void ConfirmDonation(Donation donationToConfirm);

        void UpdateDonation(Donation donationToUpdate);
        void DeleteDonation(int donationToDeleteId);
    }
}