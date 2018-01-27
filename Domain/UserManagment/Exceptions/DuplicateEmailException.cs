using Common.DomainSteroids;

namespace UserManagment.Exceptions
{
    public class DuplicateEmailException : DomainException
    {
        public DuplicateEmailException(string email)
        {
            Message = $"User with email '{email}' already exists";
            FieldName = nameof(email);
        }

        public override string Message { get; }
    }
}