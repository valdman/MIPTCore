namespace MIPTCore.Models
{
    public class AbstractUserModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string EmailAddress { get; set; }
        
        public bool IsMiptAlumni { get; set; }

        public AlumniProfileModel AlumniProfile { get; set; }
    }
}