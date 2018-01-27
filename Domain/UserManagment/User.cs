using System;
using Common.Abstractions;
using Common.Entities;

namespace UserManagment
{
    public class User : PersistentEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Password Password { get; set; }
        public bool IsMiptAlumni { get; set; }

        public AlumniProfile AlumniProfile { get; set; }
        public bool IsEmailConfirmed { get; set; }
        
        public UserRole Role { get; set; }

        public DateTimeOffset? AuthentificatedAt { get; set; }
    }
}