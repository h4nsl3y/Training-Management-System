using DAL.Entity;
using DAL.Enum;
using DAL.Logger;
using DAL.Repository.NotificationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Unity.Storage.RegistrationSet;

namespace BLL.NotificationBusinessLogics
{
    public class NotificationBusinessLogic : INotificationBusinessLogic
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger _logger;
        private readonly Response<bool> _resultBoolError;
        private readonly Response<Notification> _resultError;
        public NotificationBusinessLogic(INotificationRepository notificationRepository, ILogger logger) 
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
            _resultError = new Response<Notification> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<bool>> AddNotificationAsync(Notification notification, string email)
        {
            try
            {
                _ = Task.Run(() => SendEmail(email, notification.Subject, notification.Body));
                return await _notificationRepository.AddNotificationAsync(notification);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Response<Notification>> GetNotificationAsync(int accountId)
        {
            try
            {
                return await _notificationRepository.GetNotificationAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<bool>> UpdateStateNotificationNotificationAsync(int notificationId)
        {
            try
            {
                return await _notificationRepository.UpdateStateNotificationNotificationAsync(notificationId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public void SendEmail(string recipientEmail,string subject, string body = null)
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
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            smtpClent.Send(mailMessage);
        }
    }
}