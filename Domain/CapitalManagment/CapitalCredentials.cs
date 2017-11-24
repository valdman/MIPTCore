using Common;

namespace CapitalManagment
{
    public class CapitalCredentials : AbstractIdentifyable
    {
        public string MerchantLogin { get; set; }
        public string MerchantPassword { get; set; }
    }
}