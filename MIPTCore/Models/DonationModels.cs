using System;
using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
	public abstract class AbstractDonationModel
	{
		[Required]
		public decimal Value { get; set; }

		[Required]
		public bool IsRecursive { get; set; }
	}
	
    public class DonationWithRegistrationModel : AbstractDonationModel
    {
	    [Required]
	    public int CapitalId { get; set; }
	    
        [Required]
        public string FirstName { get; set; }

	    [Required]        
        public string LastName { get; set; }

        [Required]        
        [EmailAddress]
        public string Email { get; set; }
	    
	    [Required]
	    public bool IsMiptAlumni { get; set; }

	    public AlumniProfileModel AlumniProfile { get; set; }
    }
	
	public class ShortDonationModel : AbstractDonationModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CapitalId { get; set; }
	    
	    public bool IsConfirmed { get; set; }

	    public DateTimeOffset CreatingTime { get; set; }
    }
    
	public class ExpandedDonationModel : AbstractDonationModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public UserModel User { get; set; }

		[Required]	
		public CapitalModel Capital { get; set; }
		
		public bool IsConfirmed { get; set; }

		public DateTimeOffset CreatingTime { get; set; }
	}
    
    public class CreateDonationModel : AbstractDonationModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CapitalId { get; set; }
    }
	
	public class UpdateDonationModel : AbstractDonationModel
	{
		[Required]
		public int UserId { get; set; }

		[Required]
		public int CapitalId { get; set; }
	}
}
