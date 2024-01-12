using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.NotificationRepositories
{
    public interface INotificationRepository
    {
        Task<Response<bool>> AddNotificationAsync(Notification notification);
        Task<Response<Notification>> GetNotificationAsync(int accountId);
        Task<Response<bool>> UpdateStateNotificationNotificationAsync(int notificationId);
    }
}
