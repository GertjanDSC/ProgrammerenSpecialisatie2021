using Stock.Domain.Contracts;
using System;
// For emailing:
using System.Net;
using System.Net.Mail;

namespace Stock.Infrastructure.Mailer
{

    public class SmtpMailer : IEmailService
    {        
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public void Send(string from, string[] to, string subject, string content, bool isHtml = true)
        {
            using (SmtpClient smtpClient = new())
            {
                var basicCredentials = new NetworkCredential(User, Password);
                using (MailMessage message = new())
                {
                    MailAddress fromAddress = new(from);

                    smtpClient.Host = Host;
                    smtpClient.Port = Port;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = basicCredentials;

                    message.From = fromAddress;
                    message.Subject = subject;
                    // Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = isHtml;

                    message.Body = content;
                    foreach (var t in to)
                    {
                        message.To.Add(t);
                    }

                    try
                    {
                        smtpClient.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Domain.Services.Logger.Exception(ex);
                    }
                }
            }
        }
    }
}
