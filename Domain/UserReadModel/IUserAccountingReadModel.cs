using System.Collections.Generic;
using DonationManagment;

namespace UserReadModel
{
    public interface IUserAccountingReadModel
    {
        (IEnumerable<CapitalizationInfo>, OverallCapitalizationInfo) GetCapitalizationInfo(int userId);
        IEnumerable<Donation> GetUserDonations(int userId);
    }
}