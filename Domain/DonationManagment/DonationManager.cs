using System;
using System.Diagnostics;
using DonationManagment.Application;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment;
using Common.Infrastructure;
using Journalist;

namespace DonationManagment
{
    public class DonationManager : IDonationManager
    {
        public async Task<IEnumerable<Donation>> GetAllDonations()
        {
            return await _donationRepository.GetAll();
        }

        public Task<Donation> GetDonation(int donationId)
        {
            Require.NotNull(donationId, nameof(donationId));

            return _donationRepository.GetByIdAsync(donationId);
        }

        public async Task<int> CreateDonationAsync(Donation donationToCreate)
        {
            Require.NotNull(donationToCreate, nameof(donationToCreate));

            var id = await _donationRepository.CreateAsync(donationToCreate);

            
            if (donationToCreate.IsConfirmed)
            {
                Debug.WriteLine("Autoconfirmed donation creation");

                await ConfirmDonation(donationToCreate);
            }

            return id;
        }

        public async Task ConfirmDonation(Donation donationToConfirm)
        {
            Require.NotNull(donationToConfirm, nameof(donationToConfirm));
            var targetProject = _capitalManager.GetCapitalByIdAsync(donationToConfirm.CapitalId);

            Require.NotNull(targetProject, nameof(targetProject));

            donationToConfirm.IsConfirmed = true;
            await _donationRepository.UpdateAsync(donationToConfirm);
        }

        public async Task UpdateDonationAsync(Donation donationToUpdate)
        {
            Require.NotNull(donationToUpdate, nameof(donationToUpdate));

            var oldDonation = await _donationRepository.GetByIdAsync(donationToUpdate.Id);

            if (oldDonation.IsConfirmed != donationToUpdate.IsConfirmed)
            {
                Debug.WriteLine("Implicit donation confirmation status changing");

                if (!donationToUpdate.IsConfirmed)
                {
                    throw new RollbackDonationException();
                }

               await ConfirmDonation(donationToUpdate);

            }

            await _donationRepository.UpdateAsync(donationToUpdate);
        }

        public async Task DeleteDonation(int donationToDeleteId)
        {
            Require.NotNull(donationToDeleteId, nameof(donationToDeleteId));

            await _donationRepository.DeleteAsync(donationToDeleteId);
        }

        public async Task<IEnumerable<Donation>> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null)
        {
            return await _donationRepository.FindByAsync(predicate);
        }

        private readonly IGenericRepository<Donation> _donationRepository;
        private readonly ICapitalManager _capitalManager;

        public DonationManager(IGenericRepository<Donation> donationRepository, ICapitalManager capitalManager)
        {
            _donationRepository = donationRepository;
            _capitalManager = capitalManager;
        }
    }
}