using BLL.NotificationBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationBusinessLogic _notificationBusinessLogic;
        public NotificationController(INotificationBusinessLogic notificationBusinessLogic)
        {
            _notificationBusinessLogic = notificationBusinessLogic;
        }
        [HttpPost]
        public async Task<JsonResult> AddNotification(int accountId,int enrollmentState,string trainingTitle,string comment, string email) 
            => Json(await _notificationBusinessLogic.AddNotificationAsync(accountId, enrollmentState, trainingTitle, comment, email), JsonRequestBehavior.AllowGet);
        [HttpGet]
        public async Task<JsonResult> GetNotification()
         => Json(await _notificationBusinessLogic.GetNotificationAsync(((Account)Session["Account"]).AccountId), JsonRequestBehavior.AllowGet);

        [HttpPost]
        public async Task<JsonResult> UpdateNotificationState(int notificationId) 
            => Json(await _notificationBusinessLogic.UpdateStateNotificationNotificationAsync(notificationId), JsonRequestBehavior.AllowGet);
    }
}