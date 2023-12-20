using BLL.GenericBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class RoleController : Controller
    {
        private readonly IGenericBusinessLogic<Role> _genericBusinessLogic;
        public RoleController(IGenericBusinessLogic<Role> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
        }
        [HttpGet]
        public JsonResult GetRoleList()
        {
            Result<Role> accountResult = _genericBusinessLogic.GetAll();
            return (accountResult.Success) ?
               Json(new { message = "success", data = accountResult.Data }, JsonRequestBehavior.AllowGet) :
               Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
        }
    }
}