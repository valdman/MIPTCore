using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DonationManagment.Application
{
    public interface IDonationManager
    {
        Task<IEnumerable<Donation>> GetAllDonations();
        Task<IEnumerable<Donation>> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null);
        Task<Donation> GetDonation(int donationId);
        Task<int> CreateDonationAsync(Donation donationToCreate);
        Task ConfirmDonation(Donation donationToConfirm);

        Task UpdateDonationAsync(Donation donationToUpdate);
        Task DeleteDonation(int donationToDeleteId);
    }
}