using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using DonationManagment;
using Newtonsoft.Json;

namespace MIPTCore.Models
{
	public abstract class AbstractDonationModel
	{
		public decimal Value { get; set; }

		public bool IsRecursive { get; set; }

		public PaymentType PaymentType { get; set; }
	}
	
    public class DonationWithRegistrationModel : AbstractDonationModel
    {
	    public int CapitalId { get; set; }
	    
        public string FirstName { get; set; }
     
        public string LastName { get; set; }

        public string Email { get; set; }
	    
	    public bool IsMiptAlumni { get; set; }

	    public AlumniProfileModel AlumniProfile { get; set; }
    }
	
	public class ShortDonationModel : AbstractDonationModel
    {
	    public int Id { get; set; }
	    
        public int UserId { get; set; }

        public int CapitalId { get; set; }
	    
	    public bool IsConfirmed { get; set; }

	    public DateTimeOffset CreatingTime { get; set; }
    }
    
	public class ExpandedDonationModel : AbstractDonationModel
	{
		public int Id { get; set; }

		public UserModel User { get; set; }

		public ShortCapitalModel Capital { get; set; }
		
		public bool IsConfirmed { get; set; }
		
		[XmlElement("CreatingTime")]
		[JsonIgnore]
		public string CreatingTimeXmlString
		{
			get => CreatingTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
			set => CreatingTime = DateTimeOffset.Parse(value);
		}

		[XmlIgnore]
		public DateTimeOffset CreatingTime { get; set; }
	}
    
    public class CreateDonationModel : AbstractDonationModel
    {
        public int UserId { get; set; }

        public int CapitalId { get; set; }
    }
	
	public class UpdateDonationModel : AbstractDonationModel
	{
		public int UserId { get; set; }

		public int CapitalId { get; set; }
		
		public bool IsConfirmed { get; set; }
	}
}
