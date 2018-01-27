using Common.DomainSteroids;

namespace DonationManagment
{
    public class IvalidPaymentType : DomainException
    {
        public IvalidPaymentType()
        {
            FieldName = "PaymentType";
        }
        
        public override string Message => $"Ivalid payment type";
    }
}