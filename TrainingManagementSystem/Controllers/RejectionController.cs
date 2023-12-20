using BLL.GenericBusinessLogics;
using BLL.ViewModelsBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.ViewModels;

namespace TrainingManagementSystem.Controllers
{
    public class RejectionController : Controller
    {
        private readonly IGenericBusinessLogic<Rejection> _genericBusinessLogic;
        public RejectionController(IGenericBusinessLogic<Rejection> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
        }
        [HttpPost]
        public ActionResult SetRejectionComment(int enrollmentId, string comment)
        {
            Rejection rejection = new Rejection() { EnrollmentId = enrollmentId, Comment = comment };
            Result<bool> result = _genericBusinessLogic.Add(rejection);
            return (result.Success) ?
               Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
               Json(new { message = "failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}