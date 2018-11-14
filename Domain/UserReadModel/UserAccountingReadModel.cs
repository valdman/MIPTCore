using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Dynamic;
using CapitalManagment;
using Common.Infrastructure;
using DonationManagment;

namespace UserReadModel
{
    public class UserAccountingReadModel : IUserAccountingReadModel
    {
        private readonly IQueryable<Donation> _donationsSet;
        private readonly IQueryable<Capital> _capitalsSet;

        public UserAccountingReadModel(IGenericRepository<Donation> donationRepository, IGenericRepository<Capital> capitalsRepository)
        {
            _donationsSet = donationRepository.AsQueryable();
            _capitalsSet = capitalsRepository.AsQueryable();
        }

        public (IEnumerable<CapitalizationInfo>, CapitalizationInfo) GetCapitalizationInfo(int userId)
        {
            var userDonationsByCapital = _donationsSet
                .AsQueryable()
                .Where(d => !d.IsDeleted && d.IsConfirmed && d.UserId == userId)
                .GroupBy(d => d.CapitalId)
                .ToImmutableList();

            var capitalIdsDonatedTo = userDonationsByCapital
                .Select(group => group.Key)
                .ToImmutableHashSet();

            var capitals = _capitalsSet
                .AsQueryable()
                .Where(c => capitalIdsDonatedTo.Contains(c.Id))
                //Distinct or GroupBy with Comparer doesn't supported by npgsql driver, so materialize it
                .ToImmutableList()
                .Distinct(new Capital.HashEqualityComparer())
                .ToImmutableList();

            var donationStats = userDonationsByCapital.Select(donationGroup => 
                CountCapitalizationInfoForDonations(donationGroup, capitals.Single(c => c.Id == donationGroup.Key))).ToImmutableList();
            
            return (donationStats, new CapitalizationInfo
            {
                Donated = donationStats.Sum(d => d.Donated),
                Income = donationStats.Sum(d => d.Income),
                Spent = donationStats.Sum(d => d.Spent),
            });
        }

        IEnumerable<Donation> IUserAccountingReadModel.GetUserDonations(int userId)
        {
            return _donationsSet
                .AsQueryable()
                .Where(d => !d.IsDeleted && d.IsConfirmed && d.UserId == userId)
                .ToImmutableList();
        }

        private CapitalizationInfo CountCapitalizationInfoForDonations(IEnumerable<Donation> donations, Capital capital)
        {   
            var endOfPrevYear = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 0);
            
            
            var info = new CapitalizationInfo();
            info.CapitalId = capital.Id;
            info.CapitalName = capital.Name;
            info.CapitalDescription = capital.Description;

            info.Donated = donations.Sum(d =>
            {
                
                if (!d.IsRecursive) return d.Value;
                
                var ends = d.CancelDate ?? DateTimeOffset.Now;
                var monthPassed = decimal.Ceiling((ends - d.CreatingTime).Days / (decimal)28);

                return monthPassed * d.Value;
            });
            
            throw new NotImplementedException();
        }
    }
}