using System.Xml.Serialization;

namespace MIPTCore.Models
{
    [XmlRoot("operation")]
    public class PaymentCallbackModel
    {
        [XmlElement("order_id")]
        public string OrderId { get; set; }   
        
        [XmlElement("order_state")]
        public string OrderState { get; set; }
        
        [XmlElement("reference")]
        public int Reference { get; set; }
        
        [XmlElement("id")]
        public int Id { get; set; }
        
        [XmlElement("date")]
        public string Date { get; set; }
        
        [XmlElement("type")]
        public string Type { get; set; }
        
        [XmlElement("state")]
        public string State { get; set; }
        
        [XmlElement("reason_code")]
        public string ReasonCode { get; set; }
        
        [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("pan")]
        public string Pan { get; set; }
        
        [XmlElement("email")]
        public string Email { get; set; }
        
        [XmlElement("amount")]
        public string Amount { get; set; }
        
        [XmlElement("currency")]
        public string Currency { get; set; }
        
        [XmlElement("approval_code")]
        public string ApprovalCode { get; set; }
        
        [XmlElement("signature")]
        public string Signature { get; set; }
    }

    public enum PaymentOperationStatus
    {
        Success = 1,
        Failure = 0
    }
}