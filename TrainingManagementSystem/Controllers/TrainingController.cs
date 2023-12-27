using BLL.GenericBusinessLogics;
using BLL.TrainingBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        public async Task<JsonResult> RegisterTraining(Training training)
        {
            Result<bool> result = await _genericBusinessLogic.AddAsync(training);
            return result.Success ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetTraining(int trainingId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(primaryKey, trainingId);
            Result<Training> result = await _genericBusinessLogic.GetAsync(conditions);
            return result.Success ?
               Json(new { message = "success", data = result.Data.FirstOrDefault() }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllTraining()
        {
            Result<Training> result = await _genericBusinessLogic.GetAllAsync();
            return (result.Success) ?
               Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet] 
        public async Task<JsonResult> GetUnenrolledTraining()
        {
            int accountId = (int)Session["AccountId"];
            Result<Training> result = await _trainingBusinessLogic.GetUnenrolledAsync(accountId);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateTraining(Training training)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("Title", training.Title);
            values.Add("DepartmentId", training.DepartmentId);
            values.Add("SeatNumber", training.SeatNumber);
            values.Add("Deadline", training.Deadline);
            values.Add("StartDate", training.StartDate);
            values.Add("EndDate", training.EndDate);
            values.Add("ShortDescription", training.ShortDescription);
            values.Add("LongDescription", training.LongDescription);
            Result<bool> result = await _genericBusinessLogic.UpdateAsync(training.TrainingId,values);
            return result.Success ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> SetPrerequisite(int prerequisiteId, string title)
        {
            Result<bool> result = await _trainingBusinessLogic.SetPrerequisiteAsync(prerequisiteId, title);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}