using DAL.Entity;
using DAL.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Email
{
    public class Email : IEmail
    {
        private readonly ILogger _logger;
        public Email(ILogger logger)
        {
            _logger = logger;
        }
        public bool SendEmail(string Subject, string Body, string recipientEmail)
        {
            string senderEmail = "TrainingManagementSystem@ceridian.com";
            var smtpClent = new SmtpClient("relay.ceridian.com")
            {
                Port = 25,
                EnableSsl = true,
                UseDefaultCredentials = true,
            };

            var mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };

            try
            {
                smtpClent.Send(mailMessage);
                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return false;
            }
        }
    }
}
