using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface IUserManager
    {
        User GetUserById(int userId);
        User GetUserByEmail(string email);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetUsersByPredicate(Expression<Func<User, bool>> predicate);

        void UpdateUser(User userToUpdate);
        int CreateUser(User userToCreate);
        User GetOrSaveUserWithoutCredentials(User userToSave);

        void DeleteUser(int userToDeleteId);
    }
}