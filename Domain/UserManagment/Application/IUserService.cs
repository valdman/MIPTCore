using Common.Entities;

namespace UserManagment.Application
{
    public interface IUserService
    {
        void ConfirmEmail(string emailToConfirm);
        void ChangePassword(int userIdToChangePassword, Password newPassword);
    }
}