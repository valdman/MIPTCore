using Common;

namespace UserManagment
{
    public class Ticket : PersistentEntity
    {
        public Ticket() {}

        public TicketType TicketType { get; set; }
        public string EmailToSend { get; set; }
        public string Token { get; set; }
        public bool IsCompleted { get; set; }
    }

    public enum TicketType
    {
        EmailConfirmation = 2,
        PasswordRecovery = 3
    }
}