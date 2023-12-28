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
    public class StateController : Controller
    {
        private readonly IGenericBusinessLogic<State> _genericBusinessLogic;
        public StateController(IGenericBusinessLogic<State> genericBusinessLogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
        }
        [HttpGet]
        public async Task<JsonResult> GetStateList() => Json(await _genericBusinessLogic.GetAllAsync(), JsonRequestBehavior.AllowGet) ;
    }
}