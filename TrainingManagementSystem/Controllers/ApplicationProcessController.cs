using BLL.AccountTrainingBusinessLogics;
using BLL.ApplicationProcessBusinessLogics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class ApplicationProcessController : Controller
    {
        private readonly IApplicationProcessBusinessLogic _applicationProcessBusinessLogic;
        public ApplicationProcessController(IApplicationProcessBusinessLogic applicationProcessBusinessLogic) 
        {
            _applicationProcessBusinessLogic = applicationProcessBusinessLogic;
        }
        public async Task<JsonResult> GenerateCSVFile(int trainingId) 
            => Json(await _applicationProcessBusinessLogic.CreateExcelFile(trainingId), JsonRequestBehavior.AllowGet);

    }
}