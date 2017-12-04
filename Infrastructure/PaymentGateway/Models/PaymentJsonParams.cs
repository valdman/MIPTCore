using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace PaymentGateway.Models
{
    public class PaymentJsonParams
    {
        public int? RecurringFrequency { get; set; }
        public int? RecurringExpiry { get; set; }
        public string Email { get; set; }
        [JsonProperty("FI")]
        public string FullName { get; set; }
    }
}