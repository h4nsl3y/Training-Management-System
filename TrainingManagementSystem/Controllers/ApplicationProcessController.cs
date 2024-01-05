using BLL.AccountTrainingBusinessLogics;
using BLL.ApplicationProcessBusinessLogics;
using DAL.Entity;
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
        public async Task<ActionResult> GenerateCSVFile(int trainingId)
        {
            Response<byte[]> byteResponse = await _applicationProcessBusinessLogic.CreateExcelFile(trainingId);
            return File(byteResponse.Data.FirstOrDefault(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.csv");
        }
    }
}