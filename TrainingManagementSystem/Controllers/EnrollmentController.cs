using BLL.Email;
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
        public EnrollmentController(IGenericBusinessLogic<Enrollment> genericBusinessLogic, IEnrollmentBusinessLogic enrollmentBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _enrollmentBusinessLogic = enrollmentBusinessLogic; 
        }
        [HttpGet]
        public async Task<JsonResult> GetEnrollment(int enrollmentId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "EnrollmentId", enrollmentId } };
            return Json(await _genericBusinessLogic.GetAsync(conditions), JsonRequestBehavior.AllowGet) ;
        }
        [HttpGet]
        public async Task<JsonResult> GetAllEnrollmentByEmployee(string email)
        {
            if (email == "none") { email = Session["Email"].ToString(); }
            return Json(await _enrollmentBusinessLogic.GetEnrollmentByEmailAsync(email), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> RegisterEnrollment(int trainingId)
        {
            Enrollment enrollment = new Enrollment() {TrainingId = trainingId,AccountId = (int)Session["AccountId"], SubmissionDate = DateTime.Now};
            return Json(await _genericBusinessLogic.AddAsync(enrollment), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateState(Enrollment enrollment) 
        { 
            return Json(await _genericBusinessLogic.UpdateAsync(enrollment), JsonRequestBehavior.AllowGet);      
        }
    }
}