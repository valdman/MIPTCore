namespace UserManagment.Exceptions
{
    public class TicketAlreadyCompletedOrNotExistsException : UserVitalDomainException
    {
        public TicketAlreadyCompletedOrNotExistsException(string additionalInfo) : base(additionalInfo)
        {
        }
    }
}