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
        public async Task<JsonResult> RegisterTraining(Training training) => Json(await _genericBusinessLogic.AddAsync(training), JsonRequestBehavior.AllowGet) ;
        [HttpGet]
        public async Task<JsonResult> GetTraining(int trainingId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "TRAININGID", trainingId } };
            Response<Training> result = await _genericBusinessLogic.GetAsync(conditions);
            result.Data = new List<Training> { result.Data.FirstOrDefault() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllTraining() 
            => Json(await _genericBusinessLogic.GetAllAsync(), JsonRequestBehavior.AllowGet) ;
        [HttpGet] 
        public async Task<JsonResult> GetUnenrolledTraining() 
            => Json(await _trainingBusinessLogic.GetUnenrolledAsync((int)Session["AccountId"]), JsonRequestBehavior.AllowGet) ;

        [HttpPost]
        public async Task<JsonResult> UpdateTraining(Training training)
            => Json(await _genericBusinessLogic.UpdateAsync(training), JsonRequestBehavior.AllowGet) ;
        
        [HttpPost]
        public async Task<JsonResult> SetPrerequisite(int prerequisiteId, string title)
            =>Json(await _trainingBusinessLogic.SetPrerequisiteAsync(prerequisiteId, title), JsonRequestBehavior.AllowGet) ;
    }
}