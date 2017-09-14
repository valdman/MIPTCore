using System.ComponentModel.DataAnnotations;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserRegistrationModel : AbstractUserModel
    {
        public string Password { get; set; }
    }
}