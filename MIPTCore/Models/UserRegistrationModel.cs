using System.ComponentModel.DataAnnotations;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserRegistrationModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string EmailAddress { get; set; }
        
        public bool IsMiptAlumni { get; set; }

        public AlumniProfile AlumniProfile { get; set; }
    }
}