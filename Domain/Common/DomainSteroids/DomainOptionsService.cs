using Common.Infrastructure;

namespace Common.DomainSteroids
{
    public class DomainOptionsService : IDomainOptionsService
    {
        private readonly IDomainOptionsRepository _domainOptionsRepository;

        public DomainOptionsService(IDomainOptionsRepository domainOptionsRepository)
        {
            _domainOptionsRepository = domainOptionsRepository;
        }

        public DomainOptions GetDomainOptions()
        {
            return _domainOptionsRepository.GetDomainOptions();
        }

        public void UpdateDomainOption(DomainOptions newOptions)
        {
            var oldOptions = GetDomainOptions();
            oldOptions.SizeOfFund = newOptions.SizeOfFund;
            
            _domainOptionsRepository.UpdateDomainOptions(oldOptions);
        }
    }
}