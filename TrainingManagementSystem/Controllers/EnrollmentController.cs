using BLL.GenericBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IGenericBusinessLogic<Enrollment> _genericBusinessLogic;
        private readonly string primaryKey;
        private Result<bool> _boolResult;
        public EnrollmentController(IGenericBusinessLogic<Enrollment> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            PropertyInfo[] properties = typeof(Enrollment).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        [HttpGet]
        public JsonResult GetEnrollment(int enrollmentId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(primaryKey, enrollmentId);
            Result<Enrollment> enrollmentResult = _genericBusinessLogic.Get(conditions);
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllEnrollment()
        {
            Result<Enrollment> enrollmentResult = _genericBusinessLogic.GetAll();
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllEnrollmentWaiting()
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("STATEID", 1);
            Result<Enrollment> enrollmentResult = _genericBusinessLogic.GetAll();
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllEnrollmentByEmployee()
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ACCOUNTID", (int)Session["AccountId"]);
            Result<Enrollment> enrollmentResult = _genericBusinessLogic.GetAll(conditions);
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllEnrollmentForEmployee(int accountId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("ACCOUNTID", accountId);
            Result<Enrollment> enrollmentResult = _genericBusinessLogic.GetAll(conditions);
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RegisterEnrollment(int trainingId)
        {
            Enrollment enrollment = new Enrollment();
            enrollment.TrainingId = trainingId;
            enrollment.AccountId = (int)Session["AccountId"];
            enrollment.SubmissionDate = DateTime.Now;
            _boolResult = _genericBusinessLogic.Add(enrollment);
            return (_boolResult.Success) ?
                Json(new { message = "success" }):
                Json(new { message = "Failed", data = _boolResult.Message });
        }

        public ActionResult UpdateState(int enrollmentId, int state)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("STATEID", state);
            _boolResult = _genericBusinessLogic.Update(enrollmentId,conditions);
            return (_boolResult.Success)?
                 Json(new { message = "success" }) :
                Json(new { message = "Failed", data = _boolResult.Message });
        }
    }
}