namespace Mailer
{
    public class MailerSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string ContactAdress { get; set; }

        public bool IsMailerEnabled { get; set; }
    }
}