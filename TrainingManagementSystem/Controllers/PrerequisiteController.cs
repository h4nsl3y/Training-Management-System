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
        }
        [HttpGet]
        public async Task<JsonResult> GetAllPrerequisite() => Json(await _genericBusinessLogic.GetAllAsync(), JsonRequestBehavior.AllowGet);
        [HttpGet]
        public async Task<JsonResult> GetPrerequisite(int prerequisiteId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "PREREQUISITEID", prerequisiteId } };
            return Json(await _genericBusinessLogic.GetAsync(conditions), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrerequisiteByTraining(int trainingId) 
            => Json(await _prerequisiteBusinessLogic.GetPrequisiteAsync(trainingId), JsonRequestBehavior.AllowGet) ;

        [HttpGet]
        public async Task<JsonResult> GetPrerequisiteFile()
            => Json(await _prerequisiteBusinessLogic.GetPrerequisiteIdByEmployee(((Account)Session["Account"]).AccountId), JsonRequestBehavior.AllowGet);
 
        [HttpPost]
        public async Task<JsonResult> AddPrerequisite(string prerequisiteDescription)
        {
            Prerequisite newPrerequisite = new Prerequisite { PrerequisiteDescription = prerequisiteDescription};
            return Json(await _genericBusinessLogic.AddAsync(newPrerequisite), JsonRequestBehavior.AllowGet) ;
        }
    }
}