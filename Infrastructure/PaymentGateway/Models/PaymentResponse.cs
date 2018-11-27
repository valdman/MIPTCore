using System;
using RestSharp.Deserializers;

namespace PaymentGateway.Models
{
    public class PaymentResponse
    {
        [DeserializeAs(Name="id")]
        public string Id { get; set; }
        
        [DeserializeAs(Name="state")]
        public string State { get; set; }

        [DeserializeAs(Name="inprogress")]
        public string InProgress { get; set; }
        
        [DeserializeAs(Name="date")]
        public DateTimeOffset Date { get; set; }
        
        [DeserializeAs(Name="amount")]
        public int Amount { get; set; }
        
        [DeserializeAs(Name="currency")]
        public string Currency { get; set; }
        
        [DeserializeAs(Name="email")]
        public string Email { get; set; }
        
        [DeserializeAs(Name="phone")]
        public string Phone { get; set; }
        
        [DeserializeAs(Name="reference")]
        public string Reference { get; set; }
        
        [DeserializeAs(Name="description")]
        public string Description { get; set; }
        
        [DeserializeAs(Name="code")]
        public int Code { get; set; }
        
        [DeserializeAs(Name="url")]
        public string Url { get; set; }
        
        [DeserializeAs(Name="parameters")]
        public object Parameters { get; set; }
        
        [DeserializeAs(Name="signature")]
        public string Signature { get; set; }
    }
}