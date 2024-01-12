using DAL.DataBaseHelpers;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.NotificationRepositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDataBaseHelper<Notification> _dataBaseHelper;
        public NotificationRepository(IDataBaseHelper<Notification> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
        }
        public async Task<Response<bool>> AddNotificationAsync(Notification notification)
        {
            string query = @"INSERT INTO NOTIFICATION (DATE, ACCOUNTID, SUBJECT, BODY, HASREAD)
                            VALUES(@DATE, @ACCOUNTID, @SUBJECT, @BODY, @HASREAD);";
            List<SqlParameter> parameters = new List<SqlParameter>() { 
                new SqlParameter("@DATE", notification.Date),
                new SqlParameter("@ACCOUNTID", notification.AccountId),
                new SqlParameter("@SUBJECT", notification.Subject),
                new SqlParameter("@BODY", notification.Body),
                new SqlParameter("@HASREAD", notification.HasRead)
            };
            return await _dataBaseHelper.AffectedRowsAsync(query, parameters);
        }

        public async Task<Response<Notification>> GetNotificationAsync(int accountId)
        {
            string query = @"SELECT * FROM  NOTIFICATION
                             WHERE ACCOUNTID = @ACCOUNTID;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ACCOUNTID", accountId) };
            return await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
        }

        public async Task<Response<bool>> UpdateStateNotificationNotificationAsync(int notificationId)
        {
            string query = @"UPDATE NOTIFICATION SET HASREAD = 1 WHERE NOTIFICATIONID = @NOTIFICATIONID";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@NOTIFICATIONID", notificationId) };
            return await _dataBaseHelper.AffectedRowsAsync(query, parameters);
        }
    }
}
