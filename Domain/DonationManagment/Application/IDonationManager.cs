using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Entities.Entities.ReadModifiers;
using Common.ReadModifiers;

namespace DonationManagment.Application
{
    public interface IDonationManager
    {
        IEnumerable<Donation> GetAllDonations();
        IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate);
        
        IEnumerable<Donation> GetWithFiltersAndOrder(IEnumerable<FilteringParams> filteringParams, OrderingParams orderingParams = null);
        PaginatedList<Donation> GetPaginatedDonations(PaginationParams paginationParams, OrderingParams orderingParams, IEnumerable<FilteringParams> filteringParams, Expression<Func<Donation, bool>> predicate = null);
        
        Donation GetDonationById(int donationId);
        
        DonationPaymentInformation CreateDonationAsync(Donation donationToCreate);
        int CreateCompletedSingleDonation(Donation donationToCreate);
        void ConfirmDonation(Donation donationToConfirm);
        void CancelRecurringDonation(Donation donationToCancel);

        void UpdateDonation(Donation donationToUpdate);
        void DeleteDonation(int donationToDeleteId);
    }
}