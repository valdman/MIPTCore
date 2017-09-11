using System.ComponentModel.DataAnnotations;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        public bool IsMiptAlumni { get; set; }

        public AlumniProfile AlumniProfile { get; set; }
    }
}