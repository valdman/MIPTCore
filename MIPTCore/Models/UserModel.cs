using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using UserManagment;

namespace MIPTCore.Models
{
    public class UserModel : AbstractUserModel
    {
        public int Id { get; set; }

        public UserRole Role { get; set; }

        public DateTimeOffset CreatingTime { get; set; }
    }
}