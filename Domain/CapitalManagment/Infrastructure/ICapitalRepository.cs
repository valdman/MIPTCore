using Common.Infrastructure;

namespace CapitalManagment.Infrastructure
{
    public interface ICapitalRepository : IGenericRepository<Capital>
    {
        Capital GetCapitalByFullUri(string name);
        decimal CoutSumGivenToWholeFund();
    }
}