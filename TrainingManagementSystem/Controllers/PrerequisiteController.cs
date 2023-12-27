using BLL.AccountBusinessLogics;
using BLL.GenericBusinessLogics;
using BLL.PrerequisiteBusinesslogics;
using DAL.Entity;
using DAL.Repository.PrerequisiteRepositories;
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
    public class PrerequisiteController : Controller
    {
        private readonly IGenericBusinessLogic<Prerequisite> _genericBusinessLogic;
        private readonly IPrerequisiteBusinessLogic _prerequisiteBusinessLogic;
        public PrerequisiteController(IGenericBusinessLogic<Prerequisite> genericBusinessLogic, IPrerequisiteBusinessLogic prerequisiteBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _prerequisiteBusinessLogic = prerequisiteBusinessLogic;
            PropertyInfo[] properties = typeof(Account).GetProperties();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllPrerequisite()
        {
            Result<Prerequisite> prerequisiteResult = await _genericBusinessLogic.GetAllAsync();
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrerequisite(int prerequisiteId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            Result<Prerequisite> prerequisiteResult = await _genericBusinessLogic.GetAsync(conditions);
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrerequisiteByTraining(int trainingId)
        {
            Result<Prerequisite> prerequisiteResult = await _prerequisiteBusinessLogic.GetPrequisiteAsync(trainingId);
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrerequisiteFile()
        {
            Result<int> result = await _prerequisiteBusinessLogic.GetPrerequisiteIdByEmployee((int)Session["AccountId"]);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> AddPrerequisite(string prerequisiteDescription)
        {
            Prerequisite newPrerequisite = new Prerequisite { PrerequisiteDescription = prerequisiteDescription};
            Result<bool> result = await _genericBusinessLogic.AddAsync(newPrerequisite);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}