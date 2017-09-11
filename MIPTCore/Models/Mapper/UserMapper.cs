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
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                AlumniProfile = userModel.AlumniProfile
            };
        }
    }
}