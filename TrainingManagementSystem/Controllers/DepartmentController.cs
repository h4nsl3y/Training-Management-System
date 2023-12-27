using BLL.GenericBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IGenericBusinessLogic<Department> _genericBusinessLogic;
        public DepartmentController(IGenericBusinessLogic<Department> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
        }
        [HttpGet]
        public async Task<JsonResult> GetDepartmentList()
        {
            Result<Department> accountResult = await _genericBusinessLogic.GetAllAsync();
            return (accountResult.Success) ?
               Json(new { message = "success", data = accountResult.Data }, JsonRequestBehavior.AllowGet) :
               Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
        }
    }
}