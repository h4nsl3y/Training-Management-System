﻿using BLL.GenericBusinessLogics;
using BLL.TrainingBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class TrainingController : Controller
    {
        private readonly IGenericBusinessLogic<Training> _genericBusinessLogic;
        private readonly ITrainingBusinesslogic _trainingBusinessLogic;
        private readonly string primaryKey;
        public TrainingController(IGenericBusinessLogic<Training> genericBusinessLogic,ITrainingBusinesslogic trainingBusinesslogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _trainingBusinessLogic = trainingBusinesslogic;
            PropertyInfo[] properties = typeof(Training).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        [HttpPost]
        public async Task<JsonResult> DeleteTraining(int trainingId)
             => Json(await _trainingBusinessLogic.DeleteTrainingAsync(trainingId), JsonRequestBehavior.AllowGet);
        [HttpGet]
        public async Task<JsonResult> GetTraining(int trainingId)
            => Json(await _trainingBusinessLogic.GetTrainingAsync(trainingId), JsonRequestBehavior.AllowGet);
        [HttpGet]
        public async Task<JsonResult> GetAllTraining() 
            => Json(await _trainingBusinessLogic.GetAllTrainingAsync(), JsonRequestBehavior.AllowGet);
        [HttpGet] 
        public async Task<JsonResult> GetAvailableTraining() 
            => Json(await _trainingBusinessLogic.GetAvailableTrainingAsync((int)Session["AccountId"]), JsonRequestBehavior.AllowGet) ;
        [HttpPost]
        public async Task<JsonResult> UpdateTraining(Training training, List<int> prerequisiteList) 
            => Json(await _trainingBusinessLogic.UpdateTrainingAsync(training,prerequisiteList), JsonRequestBehavior.AllowGet);
        [HttpPost]
        public async Task<JsonResult> SetPrerequisite(int prerequisiteId, string title)
            =>Json(await _trainingBusinessLogic.SetPrerequisiteAsync(prerequisiteId, title), JsonRequestBehavior.AllowGet) ;
        [HttpPost]
        public async Task<JsonResult> RegisterTraining(Training training, List<int> prerequisiteList)
           => Json(await _trainingBusinessLogic.RegisterTrainingAsync(training, prerequisiteList), JsonRequestBehavior.AllowGet);
    }
}