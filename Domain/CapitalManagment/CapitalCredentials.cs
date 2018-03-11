using Common.Abstractions;
using Journalist.Extensions;

namespace CapitalManagment
{
    public class CapitalCredentials : AbstractIdentifyable
    {
        public string MerchantLogin { get; set; }
        public string MerchantPassword { get; set; }

        public bool IsAcquiringEnabled => MerchantLogin.IsNotNullOrEmpty() && MerchantPassword.IsNotNullOrEmpty();
    }
}