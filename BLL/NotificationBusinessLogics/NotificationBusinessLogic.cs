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
using System.Xml.Linq;
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
        public async Task<Response<bool>> AddNotificationAsync(int accountId, int enrollmentState, string trainingTitle, string comment, string email)
        {
            try
            {
                string subject = "";
                string body = "";
                switch (enrollmentState)
                {
                    case (int)EnrollmentStateEnum.Approved:
                        subject = "Approval";
                        body = $"Your request for the training <br>:'{trainingTitle}' <br>has been approved by your manager`";
                        break;
                    case (int)EnrollmentStateEnum.Cancelled:
                        subject = "Cancelled";
                        body = $"Your request for the training <br>:'{trainingTitle}' <br>has been cancelled`";
                        break;
                    case (int)EnrollmentStateEnum.Confirmed:
                        subject = "Confirmation";
                        body = $"Your enrollment reguarding the training <br>: '{trainingTitle}' <br>has been confirmed.";
                        break;
                    default:
                        subject = "Rejection";
                        body = $"Your request for the training <br>:'{trainingTitle}'<br>has been rejected due to :<br> '{comment}'";
                        break;
                }
                body = body+= "<br><br>This message is computer-generated , please do not reply.";
                _ = Task.Run(() => SendEmail(email, subject, body));

                return await _notificationRepository.AddNotificationAsync(new Notification { Subject = subject, Body = body});
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