using BLL.GenericBusinessLogics;
using BLL.TrainingBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class TrainingController : Controller
    {
        private readonly IGenericBusinessLogic<Training> _genericBusinessLogic;
        private readonly ITrainingBusinesslogic _trainingBusinessLogic;
        private readonly string primaryKey;
        public TrainingController(IGenericBusinessLogic<Training> genericBusinessLogic,ITrainingBusinesslogic trainingBusinesslogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _trainingBusinessLogic = trainingBusinesslogic;
            PropertyInfo[] properties = typeof(Training).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        [HttpPost]
        public JsonResult RegisterTraining(Training training)
        {
            Result<bool> result = _genericBusinessLogic.Add(training);
            return result.Success ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTraining(int trainingId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(primaryKey, trainingId);
            Result<Training> result = _genericBusinessLogic.Get(conditions);
            return result.Success ?
               Json(new { message = "success", data = result.Data.FirstOrDefault() }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllTraining()
        {
            Result<Training> result = _genericBusinessLogic.GetAll();
            return (result.Success) ?
               Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUnenrolledTraining()
        {
            int accountId = (int)Session["AccountId"];
            Result<Training> result = _trainingBusinessLogic.GetUnenrolled(accountId);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message}, JsonRequestBehavior.AllowGet);
        }
    }
}