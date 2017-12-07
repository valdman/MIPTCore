using System.Threading.Tasks;
using Common;

namespace UserManagment.Application
{
    public interface IUserService
    {
        void ConfirmEmail(string emailToConfirm);
        void ChangePassword(int userIdToChangePassword, Password newPassword);
    }
}