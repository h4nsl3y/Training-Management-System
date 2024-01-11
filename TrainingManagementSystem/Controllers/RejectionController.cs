using BLL.GenericBusinessLogics;
using BLL.ViewModelsBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.ViewModel;

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
        public async Task<ActionResult> SetRejectionComment(int enrollmentId, string comment)
            => Json(await _genericBusinessLogic.AddAsync(new Rejection() { EnrollmentId = enrollmentId, Comment = comment }));
    }
}