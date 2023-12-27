using BLL.Email;
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
        private readonly IEmail _emailService;
        private ILogger _logger;
        public RejectionBusinessLogic(IGenericRepository<Rejection> genericRepository, IEmail emailService, ILogger logger)
        {
            _genericRepository = genericRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<Result<bool>> RegisterRejection(int enrollmentId, string comment, string employeeEmail)
        {
            try
            {
                Result<bool> result = await _genericRepository.AddAsync(new Rejection() { EnrollmentId = enrollmentId, Comment = comment });
                /*bool emailFlag = _emailService.SendEmail("Rejection",$"Your request for the training : '' has been rejected due to \n : '{comment}'. ", employeeEmail)
                if (result.Success && emailFlag) { }*/
                return new Result<bool>() { Success = false, Message = "An error has been encounter while registering the rejection comment" };
            }
            catch(Exception exception)
            {
                _logger.Log(exception);
                return new Result<bool>() { Success = false, Message = "An error has been encounter while registering the rejection comment" };
            }
        }

    }
}
