using BLL.ViewModelsBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.ViewModels;

namespace TrainingManagementSystem.Controllers
{
    public class ViewModelController : Controller
    {
        private readonly IViewModelBusinesslogic<TrainingEnrollmentViewModel> _viewModelBusinesslogic;
        private Result<TrainingEnrollmentViewModel> _viewModelResult;
        private Result<bool> _boolResult;
        public ViewModelController(IViewModelBusinesslogic<TrainingEnrollmentViewModel> viewModelBusinesslogic)
        {
            _viewModelBusinesslogic = viewModelBusinesslogic;
            _viewModelResult = new Result<TrainingEnrollmentViewModel>();
            _boolResult = new Result<bool>();
        }
        [HttpGet]
        public JsonResult GetTrainingEnrollmentViewModel(int accountId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("AccountId", accountId);
            _viewModelResult = _viewModelBusinesslogic.GetTrainingEnrollmentView(accountId);
            return (_viewModelResult.Success) ?
               Json(new { message = "success", data = _viewModelResult.Data }, JsonRequestBehavior.AllowGet) :
               Json(new { message = "Failed", data = _viewModelResult.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}