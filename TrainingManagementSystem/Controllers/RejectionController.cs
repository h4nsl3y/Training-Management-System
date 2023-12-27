﻿using BLL.GenericBusinessLogics;
using BLL.ViewModelsBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> SetRejectionComment(int enrollmentId, string comment)
        {
            Result<bool> result = await _genericBusinessLogic.AddAsync(new Rejection() { EnrollmentId = enrollmentId, Comment = comment });
            return (result.Success) ?
               Json(new { message = "success"}, JsonRequestBehavior.AllowGet) :
               Json(new { message = "failed", data = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}