using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface IUserMailerService
    {
        Task BeginEmailConfirmation(int userId);
        Task BeginPasswordRecovery(int userId);
    }
}