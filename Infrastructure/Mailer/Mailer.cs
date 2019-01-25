using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using UserManagment;

namespace Mailer
{
    public class Mailer : IMailer
    {
        private readonly MailerSettings _mailerConfiguration;
        private readonly SmtpClient _client;

        public Mailer(IOptions<MailerSettings> mailerConfiguration)
        {
            _mailerConfiguration = mailerConfiguration.Value;
            _client = new SmtpClient(_mailerConfiguration.SmtpServer)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailerConfiguration.Login, _mailerConfiguration.Password),
                Port = _mailerConfiguration.Port != 0 ? _mailerConfiguration.Port : 25,
                EnableSsl = true
            };
        }

        public void RequestBill(User user)
        {
            var userEmail = new MailAddress(user.Email);
            var mailMessage = new MailMessage
            {
                From = userEmail,
                ReplyToList = { userEmail },
                To = {new MailAddress(_mailerConfiguration.Login)},
                Subject = $"Запрос выписки от {user.FirstName} {user.LastName}",
                Body = $"Пользователь {user.FirstName} {user.LastName} запрашивет выписку о пожертвованиях. \n" +
                       $"Email: {user.Email}, Id: {user.Id}"
            };
            
            _client.SendMailAsync(mailMessage);
        }
    }
}