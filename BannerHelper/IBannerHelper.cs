using System.Collections.Generic;

namespace BannerHelper
{
    public interface IBannerHelper
    {
        IEnumerable<BannerElement> GetBanner();
        BannerElement GetElementById(int id);

        int CreateElement(BannerElement element);
        void UpdateElement(BannerElement element);
        
        void DeleteElement(int id);
    }
}