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
        public async Task<bool> RegisterRejection(int enrollmentId, string employeeEmail, string comment)
        {
            try
            {
                Response<bool> result = await _genericRepository.AddAsync(new Rejection() { EnrollmentId = enrollmentId, Comment = comment });
               // bool emailFlag = await Task.Run(() =>_emailService.SendEmail("Rejection", $"Your request for the training has been rejected due to \n : '{comment}'. ", employeeEmail));
                return (result.Success /*&& emailFlag*/) ? true : false;
            }
            catch(Exception exception)
            {
                _logger.Log(exception);
                return false;
            }
        }

    }
}
