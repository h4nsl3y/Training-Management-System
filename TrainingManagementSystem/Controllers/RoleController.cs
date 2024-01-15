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
    public class RoleController : Controller
    {
        private readonly IGenericBusinessLogic<Role> _genericBusinessLogic;
        public RoleController(IGenericBusinessLogic<Role> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
        }
        [HttpGet]
        public async Task<JsonResult> GetRoleList() => Json(await _genericBusinessLogic.GetAllAsync(), JsonRequestBehavior.AllowGet) ;
        public async Task<JsonResult> GetUserSetRoleList()
        {
            Dictionary<string, object> dictionary= new Dictionary<string, object>() { {"UserSet", 1 } };
            return Json(await _genericBusinessLogic.GetAllAsync(dictionary), JsonRequestBehavior.AllowGet);
        }
    }
}
