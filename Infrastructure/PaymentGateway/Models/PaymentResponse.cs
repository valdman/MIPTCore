using Newtonsoft.Json;

namespace PaymentGateway.Models
{
    public class PaymentResponse
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        
        [JsonProperty("formUrl")]
        public string FormUrl { get; set; }
        
        [JsonProperty("errorCode")]
        public PaymentErrorCode ErrorCode { get; set; }
        
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}