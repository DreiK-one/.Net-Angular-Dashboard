﻿using Core.Helpers;
using Core.Interfaces;
using Core.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;


namespace Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        public void SendEmail(string email, string emailToken)
        {
            try
            {
                var emailModel = new EmailModel(email, "Reset password", EmailBody.EmailStringBody(email, emailToken));

                var emailMessage = new MimeMessage();
                var from = _configuration["EmailSettings:From"];
                emailMessage.From.Add(new MailboxAddress("NetAngularDashboard", from));
                emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
                emailMessage.Subject = emailModel.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Format(emailModel.Content)
                };

                using (var client = new SmtpClient())
                {
                    try
                    {
                        client.CheckCertificateRevocation = false;
                        client.Connect(_configuration["EmailSettings:SmtpServer"], 465, true);
                        client.Authenticate(_configuration["EmailSettings:From"], _configuration["EmailSettings:Password"]);
                        client.Send(emailMessage);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        client.Disconnect(true);
                        client.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
