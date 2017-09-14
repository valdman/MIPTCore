using Common;
using UserManagment;

namespace MIPTCore.Models.Mapper
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
                EmailAddress = user.Email,
                Role = user.Role,
                IsMiptAlumni = user.IsMiptAlumni,
                AlumniProfile = AlumniProfileToModel(user.AlumniProfile),
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
                Email = userModel.EmailAddress,
                IsMiptAlumni = userModel.IsMiptAlumni,
                AlumniProfile = AlumniProfileModelToAlumniProfile(userModel.AlumniProfile)
            };
        }

        public static User UserRegistrationModelToUser(UserRegistrationModel userRegistrationModel)
        {
            if (userRegistrationModel == null) return null;

            return new User
            {
                FirstName = userRegistrationModel.FirstName,
                LastName = userRegistrationModel.LastName,
                Email = userRegistrationModel.EmailAddress,
                Password = new Password(userRegistrationModel.Password),
                IsMiptAlumni = userRegistrationModel.IsMiptAlumni,
                AlumniProfile = AlumniProfileModelToAlumniProfile(userRegistrationModel.AlumniProfile)
            };

        }

        public static AlumniProfileModel AlumniProfileToModel(AlumniProfile profile)
        {
            if (profile == null) return null;

            return new AlumniProfileModel
            {
                Diploma = profile.Diploma,
                Faculty = profile.Faculty,
                YearOfGraduation = profile.YearOfGraduation
            };
        }
        
        public static AlumniProfile AlumniProfileModelToAlumniProfile(AlumniProfileModel profile)
        {
            if (profile == null) return null;

            return new AlumniProfile
            {
                Diploma = profile.Diploma,
                Faculty = profile.Faculty,
                YearOfGraduation = profile.YearOfGraduation
            };
        }
    }
}