namespace MIPTCore.Models
{
    public abstract class AbstractUserModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public bool IsMiptAlumni { get; set; }

        public AlumniProfileModel AlumniProfile { get; set; }
    }
}