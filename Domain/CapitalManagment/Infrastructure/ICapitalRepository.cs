using System.Threading.Tasks;
using Common.Infrastructure;

namespace CapitalManagment.Infrastructure
{
    public interface ICapitalRepository : IGenericRepository<Capital>
    {
        Task<Capital> GetCapitalByFullUriAsync(string name);
        decimal CoutSumGivenToWholeFund();
    }
}