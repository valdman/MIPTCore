using System.Threading.Tasks;
using Common;

namespace UserManagment.Application
{
    public interface IUserService
    {
        Task ConfirmEmail(string emailToConfirm);
        Task ChangePassword(int userIdToChangePassword, Password newPassword);
    }
}