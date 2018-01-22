using System.Net;
using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface IAuthentificationService
    {
        User AuthentificateAsync(Credentials credentials);
        void DeauthentificateAsync(int userId);
    }
}