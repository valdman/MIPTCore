using System;
using UserManagment;

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
    
    public class UserModel : AbstractUserModel
    {
        public int Id { get; set; }

        public UserRole Role { get; set; }

        public DateTimeOffset CreatingTime { get; set; }
    }
    
    public class UserRegistrationModel : AbstractUserModel
    {
        public string Password { get; set; }
    }
    
    public class UserUpdateModel : AbstractUserModel
    {
        //All props are inherited
    }
}