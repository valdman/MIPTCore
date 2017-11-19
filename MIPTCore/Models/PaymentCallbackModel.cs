using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
    public class PaymentCallbackModel
    {
        public string MdOrder { get; set; }
        public int OrderNumber { get; set; }
        
        [Required]
        public string Checksum { get; set; }
        public string Operation { get; set; }

        public PaymentOperationStatus Status { get; set; }
    }

    public enum PaymentOperationStatus
    {
        Success = 1,
        Failure = 0
    }
}