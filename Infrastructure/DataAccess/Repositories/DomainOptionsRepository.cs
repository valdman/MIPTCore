using System.Linq;
using Common.DomainSteroids;
using Common.Infrastructure;
using DataAccess.Contexts;

namespace DataAccess.Repositories
{
    public class DomainOptionsRepository : IDomainOptionsRepository
    {
        private readonly DomainOptionsContext _domainOptionsContext;

        public DomainOptionsRepository(DomainOptionsContext domainOptionsContext)
        {
            _domainOptionsContext = domainOptionsContext;
        }

        public DomainOptions GetDomainOptions()
        {
            return _domainOptionsContext.DomainOptionsDB.Single();
        }

        public void UpdateDomainOptions(DomainOptions newOptions)
        {
            _domainOptionsContext.DomainOptionsDB.Update(newOptions);
            _domainOptionsContext.SaveChanges();
        }
    }
}