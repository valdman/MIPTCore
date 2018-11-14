using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UserManagment;

namespace MIPTCore.Models
{
    public abstract class AbstractUserModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Bio { get; set; }

        public string Email { get; set; }
        
        public bool IsMiptAlumni { get; set; }

        public AlumniProfileModel AlumniProfile { get; set; }
    }
    
    public class UserModel : AbstractUserModel
    {
        public int Id { get; set; }

        public UserRole Role { get; set; }
        
        [XmlElement("CreatingTime")]
        [JsonIgnore]
        public string CreatingTimeXmlString
        {
            get => CreatingTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz");
            set => CreatingTime = DateTimeOffset.Parse(value);
        }

        [XmlIgnore]
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