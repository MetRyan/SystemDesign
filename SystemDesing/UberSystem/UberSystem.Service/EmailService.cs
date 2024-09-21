using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UberSytem.Dto.Requests;
using Microsoft.AspNetCore.Identity;
using UberSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace UberSystem.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettingsModel _smtpSettings;
        public EmailService(IOptions<SmtpSettingsModel> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            /*  var message = new MailMessage();
              message.From = new MailAddress(_smtpSettings.From);
              message.To.Add(new MailAddress(to));
              message.Subject = subject;
              message.Body = body;
              message.IsBodyHtml = true;

              using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
              {
                  client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                  client.EnableSsl = _smtpSettings.EnableSsl;

                  await client.SendMailAsync(message);
              }*/
            try
            {


                var message = new MailMessage();
                message.From = new MailAddress(_smtpSettings.From);

                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.EnableSsl;

                    client.SendCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                        {
                            Console.WriteLine($"Send failed: {e.Error.Message}");
                        }
                        else
                        {
                            Console.WriteLine("Email sent successfully.");
                        }
                    };
                    await client.SendMailAsync(message);


                }
            }
            catch (SmtpException ex)
            {
                // Log the details of the exception
                Console.WriteLine($"SMTP Exception: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                // Optionally log the stack trace
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Re-throw or handle the exception as needed
                throw;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"General Exception: {ex.Message}");
                // Optionally log the stack trace
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Re-throw or handle the exception as needed
                throw;
            }
        }
    }
}
