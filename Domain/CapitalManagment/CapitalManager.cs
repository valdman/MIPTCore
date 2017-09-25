using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalManagment.Exceptions;
using CapitalManagment.Infrastructure;
using Common;
using Common.Infrastructure;
using Journalist;

namespace CapitalManagment
{
    public class CapitalManager : ICapitalManager
    {
        private readonly ICapitalRepository _capitalRepository;
        private readonly DomainOptions _domainOptions;

        public CapitalManager(ICapitalRepository capitalRepository, IDomainOptionsRepository domainOptionsRepository)
        {
            _capitalRepository = capitalRepository;
            _domainOptions = domainOptionsRepository.GetDomainOptions();
        }

        public Task<Capital> GetCapitalByIdAsync(int capitalId)
        {
            Require.Positive(capitalId, nameof(capitalId));

            return _capitalRepository.GetByIdAsync(capitalId);
        }

        public Task<IEnumerable<Capital>> GetAllCapitalsAsync()
        {
            return _capitalRepository.GetAll();
        }

        public Task<IEnumerable<Capital>> GetCapitalsByPredicateAsync(Expression<Func<Capital, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return _capitalRepository.FindByAsync(predicate);
        }

        public Volume GetFundVolumeCapital()
        {
            return new Volume
            {
                Need = _domainOptions.SizeOfFund,
                Given = _capitalRepository.CoutSumGivenToWholeFund()
            };
        }

        public async Task GiveMoneyToCapitalAsync(int capitalToGiveId, decimal sumToGive)
        {
            Require.Positive(capitalToGiveId, nameof(capitalToGiveId));
            Require.True(sumToGive > 0, nameof(sumToGive), $"'{nameof(sumToGive)} is not positive'");

            var capitalToGive = await _capitalRepository.GetByIdAsync(capitalToGiveId);
            
            if(capitalToGive == null)
                throw new CapitalNotFoundException();

            capitalToGive.Given += sumToGive;

            await _capitalRepository.UpdateAsync(capitalToGive);
        }

        public Task<int> CreateCapitalAsync(Capital capitalToCreate)
        {
            Require.NotNull(capitalToCreate, nameof(capitalToCreate));

            return _capitalRepository.CreateAsync(capitalToCreate);
        }

        public Task UpdateCapitalAsync(Capital capitalToUpdate)
        {
            Require.NotNull(capitalToUpdate, nameof(capitalToUpdate));

            return _capitalRepository.UpdateAsync(capitalToUpdate);
        }

        public Task DeleteCapitalAsync(int capitalToDeleteId)
        {
            Require.Positive(capitalToDeleteId, nameof(capitalToDeleteId));

            return _capitalRepository.DeleteAsync(capitalToDeleteId);
        }
    }
}