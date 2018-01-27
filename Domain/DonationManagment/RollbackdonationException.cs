using Common.DomainSteroids;

namespace DonationManagment
{
    public class RollbackDonationException : DomainException
    {
        public RollbackDonationException()
        {
            FieldName = "rollbacking confirmed donation";
        }
    }
}