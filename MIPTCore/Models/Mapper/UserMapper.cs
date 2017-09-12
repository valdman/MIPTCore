using Common;
using UserManagment;

namespace MIPTCore.Models
{
    public static class UserMapper
    {
        public static UserModel UserToUserModel(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                AlumniProfile = user.AlumniProfile,
                CreatingDate = user.CreatedTime
            };
        }

        public static User UserModelToUser(UserModel userModel)
        {
            if (userModel == null) return null;

            return new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                IsMiptAlumni = userModel.IsMiptAlumni,
                AlumniProfile = userModel.AlumniProfile
            };
        }

        public static User UserRegistrationModelToUser(UserRegistrationModel userRegistrationModel)
        {
            if (userRegistrationModel == null) return null;

            return new User
            {
                FirstName = userRegistrationModel.FirstName,
                LastName = userRegistrationModel.LastName,
                Email = userRegistrationModel.Email,
                Password = new Password(userRegistrationModel.Password),
                IsMiptAlumni = userRegistrationModel.IsMiptAlumni,
                AlumniProfile = userRegistrationModel.AlumniProfile
            };

        }
    }
}