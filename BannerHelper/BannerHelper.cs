using System.Collections.Generic;
using Journalist;

namespace BannerHelper
{
    public class BannerHelper : IBannerHelper
    {
        private readonly IBannerRepository _bannerRepository;
        
        public BannerHelper(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }
        
        public IEnumerable<BannerElement> GetBanner()
        {
            return _bannerRepository.GetAll();
        }

        BannerElement IBannerHelper.GetElementById(int id)
        {
            Require.Positive(id, nameof(id));

            return _bannerRepository.GetById(id);
        }

        public int CreateElement(BannerElement element)
        {
            Require.NotNull(element, nameof(element));
            
            return _bannerRepository.Create(element);
        }

        public void UpdateElement(BannerElement element)
        {
            Require.NotNull(element, nameof(element));
            
            _bannerRepository.Update(element);
        }

        public void DeleteElement(int id)
        {
            Require.Positive(id, nameof(id));

            _bannerRepository.Delete(id);
        }
    }
}