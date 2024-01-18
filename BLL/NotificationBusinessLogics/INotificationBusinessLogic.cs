using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.NotificationBusinessLogics
{
    public interface INotificationBusinessLogic
    {
        Task<Response<bool>> AddNotificationAsync(int accountId, int enrollmentState, string trainingTitle, string comment, string email);
        Task<Response<Notification>> GetNotificationAsync(int accountId);
        Task<Response<bool>> UpdateStateNotificationNotificationAsync(int notificationId);
        void SendEmail(string recipientEmail, string subject, string body = null);
    }
}
