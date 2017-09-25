using System.Linq;
using Common;
using Common.Infrastructure;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}