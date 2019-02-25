using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        public (IEnumerable<CapitalizationInfo>, OverallCapitalizationInfo) GetCapitalizationInfo(int userId)
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
            
            return (donationStats, new OverallCapitalizationInfo
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
            var capitalizations = capital.Capitalizations ?? new Capitalization[0];
            var donationsMaterialized = donations?.ToList() ?? new List<Donation>();
            
            return new CapitalizationInfo
            {
                CapitalId = capital.Id,
                CapitalName = capital.Name,
                CapitalDescription = capital.Description,
                Donated = donationsMaterialized.Sum(d =>
                {
                    if (!d.IsRecursive) return d.Value;

                    var ends = d.CancelDate ?? DateTimeOffset.Now;
                    var monthPassed = decimal.Ceiling((ends - d.CreatingTime).Days / (decimal) 28);

                    return monthPassed * d.Value;
                }),
                Income = donationsMaterialized.Sum(d =>
                {
                    if (d.IsRecursive) return CalculateRecurrentDonationIncome(d);

                    var cap = capitalizations.SingleOrDefault(c => c?.Year == d.CreatingTime.Year);
                    var incomePercentage = cap?.IncomePercentage ?? Capitalization.DefaultPercentage;

                    return (d.Value * incomePercentage) / (decimal) 100;
                }),
                Spent = donationsMaterialized.Sum(d =>
                {
                    if (d.IsRecursive) return CalculateRecurrentDonationSpent(d);

                    var cap = capitalizations.SingleOrDefault(c => c?.Year == d.CreatingTime.Year);
                    var spentPercentage = cap?.SpentPercentage ?? Capitalization.DefaultSpentPercentage;

                    return (d.Value * spentPercentage) / (decimal) 100;
                }),
            };
        }

        private decimal CalculateRecurrentDonationSpent(Donation donation)
        {
            throw new NotImplementedException();
        }

        private decimal CalculateRecurrentDonationIncome(Donation donation)
        {
            throw new NotImplementedException();
        }
    }
}