using Common.DomainSteroids;

namespace DonationManagment
{
    public class IvalidDonationTarget : DomainException
    {
        public IvalidDonationTarget()
        {
            FieldName = "UserId or CapitalId";
        }
        
        public override string Message => $"Trying To donate To capital that not exists/from user that not exists";
    }
}