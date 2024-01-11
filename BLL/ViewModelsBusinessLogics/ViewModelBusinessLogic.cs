using DAL.Entity;
using DAL.Logger;
using DAL.Repository.ViewModelRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BLL.ViewModelsBusinessLogics
{
    public class ViewModelBusinessLogic<T> : IViewModelBusinesslogic<T>
    {
        private readonly IViewModelRepository<T> _viewModelRepository;
        private readonly ILogger _logger;
        private readonly Response<T> _resultError;
        public ViewModelBusinessLogic(IViewModelRepository<T> viewModelRepository, ILogger logger)
        {
            _viewModelRepository = viewModelRepository;
            _logger = logger;
            _resultError = new Response<T> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<T>> GetTrainingEnrollmentView(int accountId)
        {
            try
            {
                return await _viewModelRepository.GetTrainingEnrollmentView(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
    }
}
