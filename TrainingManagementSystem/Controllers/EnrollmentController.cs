using BLL.EnrollmentBusinesslogics;
using BLL.GenericBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IGenericBusinessLogic<Enrollment> _genericBusinessLogic;
        private readonly IEnrollmentBusinessLogic _enrollmentBusinessLogic;
        private Result<bool> _boolResult;
        public EnrollmentController(IGenericBusinessLogic<Enrollment> genericBusinessLogic, IEnrollmentBusinessLogic enrollmentBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _enrollmentBusinessLogic = enrollmentBusinessLogic; 
        }
        [HttpGet]
        public async Task<JsonResult> GetEnrollment(int enrollmentId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("EnrollmentId", enrollmentId);
            Result<Enrollment> enrollmentResult = await _genericBusinessLogic.GetAsync(conditions);
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllEnrollmentByEmployee(string email)
        {
            if (email == "none") { email = Session["Email"].ToString(); }
            Result<Enrollment> enrollmentResult = await _enrollmentBusinessLogic.GetEnrollmentByEmailAsync(email);
            return (enrollmentResult.Success) ?
                Json(new { message = "success", data = enrollmentResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = enrollmentResult.Message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> RegisterEnrollment(int trainingId)
        {
            Enrollment enrollment = new Enrollment();
            enrollment.TrainingId = trainingId;
            enrollment.AccountId = (int)Session["AccountId"];
            enrollment.SubmissionDate = DateTime.Now;
            _boolResult = await _genericBusinessLogic.AddAsync(enrollment);
            return (_boolResult.Success) ?
                Json(new { message = "success" }):
                Json(new { message = "Failed", data = _boolResult.Message });
        }
        [HttpPost]
        public async Task<ActionResult> UpdateState(int enrollmentId, int state)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("STATEID", state);
            _boolResult = await _genericBusinessLogic.UpdateAsync(enrollmentId,conditions);
            return (_boolResult.Success)?
                 Json(new { message = "success" }) :
                Json(new { message = "Failed", data = _boolResult.Message });
        }
    }
}