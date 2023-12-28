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
        public async Task<JsonResult> GetDepartmentList() => Json( await _genericBusinessLogic.GetAllAsync(), JsonRequestBehavior.AllowGet);
    }
}