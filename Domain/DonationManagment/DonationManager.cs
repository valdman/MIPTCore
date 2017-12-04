using System;
using System.Diagnostics;
using DonationManagment.Application;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment;
using Common;
using Common.Infrastructure;
using DonationManagment.Infrastructure;
using Journalist;
using UserManagment.Application;

namespace DonationManagment
{
    public class DonationManager : IDonationManager
    {
        public IEnumerable<Donation> GetAllDonations()
        {
            return _donationRepository.GetAll();
        }

        public Donation GetDonationByIdAsync(int donationId)
        {
            Require.NotNull(donationId, nameof(donationId));

            return _donationRepository.GetById(donationId);
        }

        public DonationPaymentInformation CreateDonationAsync(Donation donationToCreate)
        {
            Require.NotNull(donationToCreate, nameof(donationToCreate));
            
            if(donationToCreate.PaymentType == PaymentType.Unknown)
                throw new IvalidPaymentType();
            
            var capitalToProvideDonation = _capitalManager.GetCapitalById(donationToCreate.CapitalId);
            var intendedUser = _userManager.GetUserById(donationToCreate.UserId);

            if (capitalToProvideDonation == null || intendedUser == null)
            {
                throw new IvalidDonationTarget();
            }

            _donationRepository.Create(donationToCreate);

            
            if (donationToCreate.IsConfirmed)
            {
                Debug.WriteLine("Autoconfirmed donation creation");

                ConfirmDonation(donationToCreate);
            }

            if (donationToCreate.PaymentType == PaymentType.ByBankTransfer)
                return new DonationPaymentInformation(donationToCreate.Id, null, null);

            //todo: async
            var donationFromBank = donationToCreate.IsRecursive
                ? _paymentProvider.InitiateRequrrentPaymentForDonation(donationToCreate, 
                    capitalToProvideDonation.CapitalCredentials, intendedUser)
                : _paymentProvider.InitiateSinglePaymentForDonation(donationToCreate,
                    capitalToProvideDonation.CapitalCredentials, intendedUser);

            donationToCreate.BankOrderId = donationFromBank.OrderId;
            _donationRepository.Update(donationToCreate);

            return donationFromBank;
        }

        public int CreateCompletedSingleDonation(Donation donationToCreate)
        {
            Require.NotNull(donationToCreate, nameof(donationToCreate));

            donationToCreate.IsConfirmed = true;
            donationToCreate.IsRecursive = false;

            return _donationRepository.Create(donationToCreate);
        }

        public void ConfirmDonation(Donation donationToConfirm)
        {
            Require.NotNull(donationToConfirm, nameof(donationToConfirm));
            var targetProject = _capitalManager.GetCapitalById(donationToConfirm.CapitalId);

            Require.NotNull(targetProject, nameof(targetProject));

            donationToConfirm.IsConfirmed = true;
            _donationRepository.Update(donationToConfirm);
        }

        public void UpdateDonationAsync(Donation donationToUpdate)
        {
            Require.NotNull(donationToUpdate, nameof(donationToUpdate));

            var oldDonation = _donationRepository.GetById(donationToUpdate.Id);

            if (oldDonation.IsConfirmed != donationToUpdate.IsConfirmed)
            {
                Debug.WriteLine("Implicit donation confirmation status changing");

                if (!donationToUpdate.IsConfirmed)
                {
                    throw new RollbackDonationException();
                }

               ConfirmDonation(donationToUpdate);

            }

            _donationRepository.Update(donationToUpdate);
        }

        public void DeleteDonation(int donationToDeleteId)
        {
            Require.NotNull(donationToDeleteId, nameof(donationToDeleteId));

            _donationRepository.Delete(donationToDeleteId);
        }

        public IEnumerable<Donation> GetDonationsByPredicate(Expression<Func<Donation, bool>> predicate = null)
        {
            return _donationRepository.FindBy(predicate);
        }

        private readonly IGenericRepository<Donation> _donationRepository;
        private readonly ICapitalManager _capitalManager;
        private readonly IPaymentProvider _paymentProvider;
        private readonly IUserManager _userManager;

        public DonationManager(IGenericRepository<Donation> donationRepository, ICapitalManager capitalManager, IPaymentProvider paymentProvider, IUserManager userManager)
        {
            _donationRepository = donationRepository;
            _capitalManager = capitalManager;
            _paymentProvider = paymentProvider;
            _userManager = userManager;
        }
    }
}