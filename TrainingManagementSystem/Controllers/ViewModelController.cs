using BLL.ViewModelsBusinessLogics;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.ViewModel;

namespace TrainingManagementSystem.Controllers
{
    public class ViewModelController : Controller
    {
        private readonly IViewModelBusinessLogic<TrainingEnrollmentViewModel> _viewModelBusinesslogic;
        public ViewModelController(IViewModelBusinessLogic<TrainingEnrollmentViewModel> viewModelBusinesslogic)
        {
            _viewModelBusinesslogic = viewModelBusinesslogic;
        }
        [HttpGet]
        public async Task<JsonResult> GetTrainingEnrollmentViewModel(int accountId) 
            => Json(await _viewModelBusinesslogic.GetTrainingEnrollmentView(accountId), JsonRequestBehavior.AllowGet) ;
    }
}