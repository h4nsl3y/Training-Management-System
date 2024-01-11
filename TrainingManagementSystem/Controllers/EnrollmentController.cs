using BLL.AccountTrainingBusinessLogics;
using BLL.EnrollmentBusinesslogics;
using BLL.GenericBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IGenericBusinessLogic<Enrollment> _genericBusinessLogic;
        private readonly IEnrollmentBusinessLogic _enrollmentBusinessLogic;
        private readonly IApplicationProcessBusinessLogic _applicationProcessBusinessLogic;
        public EnrollmentController(IGenericBusinessLogic<Enrollment> genericBusinessLogic, 
                                    IEnrollmentBusinessLogic enrollmentBusinessLogic,
                                    IApplicationProcessBusinessLogic applicationProcessBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _enrollmentBusinessLogic = enrollmentBusinessLogic; 
            _applicationProcessBusinessLogic = applicationProcessBusinessLogic;
        }
        [HttpGet]
        public async Task<JsonResult> GetEnrollment(int enrollmentId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>() { { "EnrollmentId", enrollmentId } };
            return Json(await _genericBusinessLogic.GetAsync(conditions), JsonRequestBehavior.AllowGet) ;
        }
        [HttpGet]
        public async Task<JsonResult> GetAllEnrollmentByEmployeeId(int accountId)
        {
            if (accountId == 0) { accountId = (int) Session["AccountId"]; }
            return Json(await _enrollmentBusinessLogic.GetEnrollmentByAccountAsync(accountId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> RegisterEnrollment(int trainingId)
        {
            Enrollment enrollment = new Enrollment() {TrainingId = trainingId,AccountId = (int)Session["AccountId"], SubmissionDate = DateTime.Now};
            return Json(await _genericBusinessLogic.AddAsync(enrollment), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateState(Enrollment enrollment,string trainingTitle, string email, string comment ) 
        {
            _ = Task.Run(() => _applicationProcessBusinessLogic.SendEmail(enrollment, trainingTitle, email, comment));
            return Json(await _genericBusinessLogic.UpdateAsync(enrollment), JsonRequestBehavior.AllowGet);
        }
    }
}