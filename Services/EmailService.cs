using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestForGraduates.Interfaces;

namespace TestForGraduates.Services
{
    public class EmailService : IMessageEmailService
    {
        public async Task SendEmailMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            // от кого
            emailMessage.From.Add(new MailboxAddress("senya", "@ от кого"));
            // кому
            emailMessage.To.Add(new MailboxAddress("", email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465);
                await client.AuthenticateAsync("Your@gmail.com", "YourPassword");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
