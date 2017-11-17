using System;
using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
	public abstract class AbstractDonationModel
	{
		[Required]
		public decimal Value { get; set; }

		public DateTimeOffset Date { get; set; }

		public bool Recursive { get; set; }
		
		public bool Confirmed { get; set; }
	}
	
    public class DonationWithRegistrationModel : AbstractDonationModel
    {
        [Required]
        public int ProjectId { get;  set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        
        [Required]        
        public string LastName { get; set; }

        [Required]        
        [EmailAddress]
        public string Email { get; set; }
    }
    
    public class SaveDonationModel : AbstractDonationModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CapitalId { get; set; }

        public DateTime CreatingDate { get; set; }
    }
    
    public class ExpandedDonationModel : AbstractDonationModel
    {
		[Required]
		public int Id { get; set; }

        [Required]
        public UserModel User { get; set; }

		[Required]	
		public CapitalModel Capital { get; set; }

		public DateTime CreatingDate { get; set; }
    }
}
