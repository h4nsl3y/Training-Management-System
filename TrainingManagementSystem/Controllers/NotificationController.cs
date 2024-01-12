using BLL.NotificationBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<JsonResult> AddNotification(Notification notification,string email) 
            => Json(await _notificationBusinessLogic.AddNotificationAsync(notification,email), JsonRequestBehavior.AllowGet);
        [HttpGet]
        public async Task<JsonResult> GetNotification() 
            => Json(await _notificationBusinessLogic.GetNotificationAsync((int)Session["AccountId"]), JsonRequestBehavior.AllowGet);
        [HttpPost]
        public async Task<JsonResult> UpdateNotificationState(int notificationId) 
            => Json(await _notificationBusinessLogic.UpdateStateNotificationNotificationAsync(notificationId), JsonRequestBehavior.AllowGet);
    }
}
/*
string subject = "";
string body = "";
switch (enrollment.StateId)
{
    case (short)EnrollmentStateEnum.Approved:
        subject = "Approval";
        body = $"Your request for the training :{trainingTitle} has been approved by your manager";
        break;
    case (short)EnrollmentStateEnum.Confirmed:
        subject = "Confirmation";
        body = $"Your enrollment reguarding the training :{trainingTitle} has been confirmed.";
        break;
    default:
        subject = "Rejection";
        body = $"Your request for the training :{trainingTitle} has been rejected due to :\n '{comment}'";
        break;
}
_ = Task.Run(() => _notificationBusinessLogic.SendEmail(email, subject, body));*/