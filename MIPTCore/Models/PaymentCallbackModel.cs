using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
    public class PaymentCallbackModel
    {
        [Required]
        public string MdOrder { get; set; }
        
        [Required]
        public int OrderNumber { get; set; }
        
        public string Checksum { get; set; }
        
        [Required]
        public string Operation { get; set; }

        [Required]
        public PaymentOperationStatus Status { get; set; }
    }

    public enum PaymentOperationStatus
    {
        Success = 1,
        Failure = 0
    }
}