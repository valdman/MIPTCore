using Common.Abstractions;

namespace BannerHelper
{
    public class BannerElement : AbstractIdentifyable
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Position { get; set; }
        public string Type { get; set; }
    }
}