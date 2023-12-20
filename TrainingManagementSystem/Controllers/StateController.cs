﻿using BLL.GenericBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public JsonResult GetStateList()
        {
            Result<State> result = _genericBusinessLogic.GetAll();
            return (result.Success) ?
               Json(new { message = "success", data = result.Data }, JsonRequestBehavior.AllowGet) :
               Json(new { message = "failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}