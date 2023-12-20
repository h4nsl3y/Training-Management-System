using DAL.Entity;
using DAL.Logger;
using DAL.Repository.ViewModelRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModelsBusinessLogics
{
    public class ViewModelBusinessLogic<T> : IViewModelBusinesslogic<T>
    {
        private readonly IViewModelRepository<T> _viewModelRepository;
        private ILogger _logger;
        private Result<T> _resultError;
        private Result<bool> _resultBoolError;
        public ViewModelBusinessLogic(IViewModelRepository<T> viewModelRepository, ILogger logger)
        {
            _viewModelRepository = viewModelRepository;
            _logger = logger;
            _resultError = new Result<T> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Result<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public Result<T> GetTrainingEnrollmentView(int accountId)
        {
            try
            {
                return _viewModelRepository.GetTrainingEnrollmentView(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
    }
}
