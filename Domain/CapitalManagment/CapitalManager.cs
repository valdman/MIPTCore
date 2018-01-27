using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CapitalManagment.Exceptions;
using CapitalManagment.Infrastructure;
using Common.DomainSteroids;
using Journalist;

namespace CapitalManagment
{
    public class CapitalManager : ICapitalManager
    {
        private readonly ICapitalRepository _capitalRepository;
        private readonly IDomainOptionsService _domainOptionsService;
        private readonly DomainOptions _domainOptions;
        
        public CapitalManager(ICapitalRepository capitalRepository, IDomainOptionsService domainOptionsService)
        {
            _capitalRepository = capitalRepository;
            _domainOptionsService = domainOptionsService;
            _domainOptions = _domainOptionsService.GetDomainOptions();
        }

        public Capital GetCapitalById(int capitalId)
        {
            Require.Positive(capitalId, nameof(capitalId));

            return _capitalRepository.GetById(capitalId);
        }

        public Capital GetCapitalByFullUri(string capitalName)
        {
            Require.NotEmpty(capitalName, nameof(capitalName));
            
            if (capitalName.Last() == '/')
                capitalName = capitalName.Remove(capitalName.Length - 1);

            return _capitalRepository.GetCapitalByFullUri(capitalName);
        }

        public IEnumerable<Capital> GetAllCapitals()
        {
            return _capitalRepository.GetAll();
        }

        public IEnumerable<Capital> GetCapitalsByPredicate(Expression<Func<Capital, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return _capitalRepository.FindBy(predicate);
        }

        public Volume GetFundVolumeCapital()
        {
            return new Volume
            {
                Need = _domainOptions.SizeOfFund,
                Given = _capitalRepository.CoutSumGivenToWholeFund()
            };
        }

        public void GiveMoneyToCapitalAsync(int capitalToGiveId, decimal sumToGive)
        {
            Require.Positive(capitalToGiveId, nameof(capitalToGiveId));
            Require.True(sumToGive > 0, nameof(sumToGive), $"'{nameof(sumToGive)} is not positive'");

            var capitalToGive = _capitalRepository.GetById(capitalToGiveId);
            
            if(capitalToGive == null)
                throw new CapitalNotFoundException();

            capitalToGive.Given += sumToGive;

            _capitalRepository.Update(capitalToGive);
        }

        public int CreateCapital(Capital capitalToCreate)
        {
            Require.NotNull(capitalToCreate, nameof(capitalToCreate));

            return _capitalRepository.Create(capitalToCreate);
        }

        public void UpdateCapital(Capital capitalToUpdate)
        {
            Require.NotNull(capitalToUpdate, nameof(capitalToUpdate));

            _capitalRepository.Update(capitalToUpdate);
        }

        public void DeleteCapital(int capitalToDeleteId)
        {
            Require.Positive(capitalToDeleteId, nameof(capitalToDeleteId));

            _capitalRepository.Delete(capitalToDeleteId);
        }
    }
}