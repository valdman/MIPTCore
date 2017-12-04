using Common;

namespace DonationManagment
{
    public class IvalidDonationTarget : DomainException
    {
        public IvalidDonationTarget()
        {
            FieldName = "UserId or CapitalId";
        }
        
        public override string Message => $"Trying to donate to capital that not exists/from user that not exists";
    }
}