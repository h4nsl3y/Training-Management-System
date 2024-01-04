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
        private readonly IViewModelBusinesslogic<TrainingEnrollmentViewModel> _viewModelBusinesslogic;
        private Response<TrainingEnrollmentViewModel> _viewModelResult;
        private Response<bool> _boolResult;
        public ViewModelController(IViewModelBusinesslogic<TrainingEnrollmentViewModel> viewModelBusinesslogic)
        {
            _viewModelBusinesslogic = viewModelBusinesslogic;
            _viewModelResult = new Response<TrainingEnrollmentViewModel>();
            _boolResult = new Response<bool>();
        }
        [HttpGet]
        public async Task<JsonResult> GetTrainingEnrollmentViewModel(int accountId) 
            => Json(await _viewModelBusinesslogic.GetTrainingEnrollmentView(accountId), JsonRequestBehavior.AllowGet) ;
    }
}