using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public bool IsIsMiptAlumni { get; set; }
        
        public AlumniProfile AlumniProfile { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatingDate { get; set; }
    }
}