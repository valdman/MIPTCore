using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public UserRole Role { get; set; }

        public bool IsMiptAlumni { get; set; }
        
        public AlumniProfile AlumniProfile { get; set; }

        public DateTimeOffset CreatingDate { get; set; }
    }
}