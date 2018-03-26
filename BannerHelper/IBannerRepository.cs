using Common.Infrastructure;

namespace BannerHelper
{
    public interface IBannerRepository : IGenericRepository<BannerElement>
    {
        void DeleteAllBannerElements();
    }
}