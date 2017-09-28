using System.Threading.Tasks;
using Common;

namespace UserManagment
{
    public interface IUserMailerService
    {
        Task BeginEmailConfirmation(int userId);
        Task BeginPasswordRecovery(int userId);

        Task ConfirmEmail(string emailToConfirm);
        Task ChangePassword(int userIdToChangePassword, Password newPassword);
        
        Task<string> GetUserEmailByPasswordRecoveyToken(string token);
        Task<string> GetUserEmailByEmailConfirmationToken(string token);
    }
}