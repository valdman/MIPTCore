using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UserManagment;
using UserManagment.Infrastructure;

namespace Mailer
{
    public class TicketSender : ITicketSender
    {
        private readonly MailerSettings _mailerConfiguration;
        private readonly SmtpClient _client;

        public TicketSender(IOptions<MailerSettings> mailerConfiguration)
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

        public Task SendTicket(string email, Ticket ticketToSend)
        {
            if (!_mailerConfiguration.IsMailerEnabled)
                return Task.CompletedTask;
            
            var mailMessage = InitMailMessageTo(email);
            AddMessageBodyForTicket(ref mailMessage, ticketToSend);
            
            return _client.SendMailAsync(mailMessage);
        }

        private void AddMessageBodyForTicket(ref MailMessage mail, Ticket ticket)
        {
            switch (ticket.TicketType)
            {
                case TicketType.EmailConfirmation:
                    mail.Subject = "Подтверждение почты на сайте Эндаумент Фонда МФТИ";
                    mail.Body = $"Ваш токен для подтверждения почты, сэр: {ticket.Token}";
                    break;
                case TicketType.PasswordRecovery:
                    mail.Subject = "Восстановление пароля на сайте Эндаумент Фонда МФТИ";
                    mail.Body = $"Для восстановления пароля на сайте Эндаумент фонда МФТИ {ticket.Token}";
                    break;
                case TicketType.CombinatedTicket:
                    mail.Subject = "Регистрация на сайте Эндаумент Фонда МФТИ";
                    mail.Body = $"Эндаумент фонд МФТИ благодарит Вас за внесенный вклад! Для подтверждения почты и установления пароля пройдите по ссылке: http://fund.mipt.ru/user-confirmation/{ticket.Token}";
                break;
                    
                default:
                    throw new ArgumentException("Trying To send ivalid ticket type");
            }
        }

        private MailMessage InitMailMessageTo(string email)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailerConfiguration.Login),
                To = {new MailAddress(email)}
            };

            return mailMessage;
        }
    }
}