using Common;

namespace UserManagment
{
    public class Ticket : PersistentEntity
    {
        public Ticket(TicketType ticketType)
        {
            TicketType = ticketType;
        }
        
        public Ticket() {}

        public TicketType TicketType { get; private set; }
        public string EmailToSend { get; set; }
        public string Token { get; set; }
        public bool IsCompleted { get; set; }
    }

    public enum TicketType
    {
        EmailConfirmation,
        PasswordRecovery
    }
}