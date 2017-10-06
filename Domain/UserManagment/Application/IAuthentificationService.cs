using System.Net;
using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface IAuthentificationService
    {
        Task<User> AuthentificateAsync(Credentials credentials);
        Task DeauthentificateAsync(int userId);
    }
}