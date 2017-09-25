using System.Threading.Tasks;
using Common.Infrastructure;

namespace CapitalManagment.Infrastructure
{
    public interface ICapitalRepository : IGenericRepository<Capital>
    {
        Task<Capital> GetcapitalByNameAsync(string name);
        decimal CoutSumGivenToWholeFund();
    }
}