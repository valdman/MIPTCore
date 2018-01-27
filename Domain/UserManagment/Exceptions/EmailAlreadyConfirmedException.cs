using Common.DomainSteroids;

namespace UserManagment.Exceptions
{
    public class EmailAlreadyConfirmedException : DomainException
    {
        public EmailAlreadyConfirmedException()
        {
            FieldName = "IsEmailconfirmed";
        }
        
        public override string Message => $"User email already confirmed";
    }
}