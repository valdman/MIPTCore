using System;
using System.Security.Claims;
using System.Security.Principal;
using Common;

namespace UserManagment
{
    public class User : PersistentEntity, IIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual Password Password { get; set; }
        public bool IsMiptAlumni { get; set; }

        public virtual AlumniProfile AlumniProfile { get; set; }
        public UserRole Role { get; set; }

        public DateTimeOffset? AuthentificatedAt { get; set; }

        public string AuthenticationType => $"Defaul auth type";

        public bool IsAuthenticated => AuthentificatedAt != null;

        public string Name => $"{FirstName} {LastName}";
    }
}