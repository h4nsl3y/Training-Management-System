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
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class PrerequisiteController : Controller
    {
        private readonly IGenericBusinessLogic<Prerequisite> _genericBusinessLogic;
        private readonly IPrerequisiteBusinessLogic _prerequisiteBusinessLogic;
        private readonly string primaryKey;
        public PrerequisiteController(IGenericBusinessLogic<Prerequisite> genericBusinessLogic, IPrerequisiteBusinessLogic prerequisiteBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _prerequisiteBusinessLogic = prerequisiteBusinessLogic;
            PropertyInfo[] properties = typeof(Account).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        [HttpGet]
        public JsonResult GetAllPrerequisite()
        {
            Result<Prerequisite> prerequisiteResult = _genericBusinessLogic.GetAll();
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPrerequisite(int prerequisiteId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("PREREQUISITEID", prerequisiteId);
            Result<Prerequisite> prerequisiteResult = _genericBusinessLogic.Get(conditions);
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPrerequisiteByTraining(int trainingId)
        {
            Result<Prerequisite> prerequisiteResult = _prerequisiteBusinessLogic.GetPrequisite(trainingId);
            return (prerequisiteResult.Success) ?
                Json(new { message = "success", data = prerequisiteResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = prerequisiteResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPrerequisiteFile()
        {
            Result<int> result = _prerequisiteBusinessLogic.GetPrerequisiteIdByEmployee((int)Session["AccountId"]);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddPrerequisite(Prerequisite prerequisite)
        {
            Result<bool> result = _genericBusinessLogic.Add(prerequisite);
            return (result.Success) ?
                Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}