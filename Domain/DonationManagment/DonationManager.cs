using System;
using System.Diagnostics;
using DonationManagment.Application;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment;
using Common.Infrastructure;
using DonationManagment.Infrastructure;
using Journalist;

namespace DonationManagment
{
    public class DonationManager : IDonationManager
    {
        public async Task<IEnumerable<Donation>> GetAllDonations()
        {
            return await _donationRepository.GetAll();
        }

        public Task<Donation> GetDonationByIdAsync(int donationId)
        {
            Require.NotNull(donationId, nameof(donationId));

            return _donationRepository.GetByIdAsync(donationId);
        }

        public async Task<DonationPaymentInformation> CreateDonationAsync(Donation donationToCreate)
        {
            Require.NotNull(donationToCreate, nameof(donationToCreate));

            var id = await _donationRepository.CreateAsync(donationToCreate);

            
            if (donationToCreate.IsConfirmed)
            {
                Debug.WriteLine("Autoconfirmed donation creation");

                await ConfirmDonation(donationToCreate);
            }

            return _paymentProvider.InitiateSinglePaymentForDonation(donationToCreate);;
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
        private readonly IPaymentProvider _paymentProvider;

        public DonationManager(IGenericRepository<Donation> donationRepository, ICapitalManager capitalManager, IPaymentProvider paymentProvider)
        {
            _donationRepository = donationRepository;
            _capitalManager = capitalManager;
            _paymentProvider = paymentProvider;
        }
    }
}