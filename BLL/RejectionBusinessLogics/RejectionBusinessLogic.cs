using BLL.AccountTrainingBusinessLogics;
using DAL.Entity;
using DAL.Logger;
using DAL.Repository.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RejectionBusinessLogics
{

    public class RejectionBusinessLogic : IRejectionBusinessLogic
    {
        private readonly IGenericRepository<Rejection> _genericRepository;
        private readonly IApplicationProcessBusinessLogic _applicationProcessBusinessLogic;
        private ILogger _logger;
        public RejectionBusinessLogic(IGenericRepository<Rejection> genericRepository, 
            IApplicationProcessBusinessLogic applicationProcessBusinessLogic, ILogger logger)
        {
            _genericRepository = genericRepository;
            _applicationProcessBusinessLogic = applicationProcessBusinessLogic;
            _logger = logger;
        }
        public async Task<Response<bool>> RegisterRejection(int enrollmentId, string employeeEmail, string comment)
        {
            try
            {
                Response<bool> result = await _genericRepository.AddAsync(new Rejection() { EnrollmentId = enrollmentId, Comment = comment });
                await Task.Run(() => _applicationProcessBusinessLogic.SendEmail("Rejection",
                                                                                $"Your request for the training has been rejected due to \n : '{comment}'. ", employeeEmail));
                return new Response<bool>() { Success = ((result.Success) ? true : false) };
            }
            catch(Exception exception)
            {
                _logger.Log(exception);
                return new Response<bool>() { Success = false , Message = "Email has not been." };
            }
        }

    }
}
