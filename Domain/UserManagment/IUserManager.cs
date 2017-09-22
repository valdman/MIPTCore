using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserManagment
{
    public interface IUserManager
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByPredicateAsync(Expression<Func<User, bool>> predicate);

        Task UpdateUserAsync(User userToUpdate);
        Task<int> CreateUserAsync(User userToCreate);

        Task DeleteUserAsync(int userToDeleteId);
    }
}