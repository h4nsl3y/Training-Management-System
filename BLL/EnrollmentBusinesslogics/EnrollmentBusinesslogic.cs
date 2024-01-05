using DAL.Entity;
using DAL.Logger;
using DAL.Repository.EnrollmentRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EnrollmentBusinesslogics
{
    public class EnrollmentBusinesslogic : IEnrollmentBusinessLogic
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ILogger _logger;
        public EnrollmentBusinesslogic(IEnrollmentRepository enrollmenyRepository, ILogger logger)
        {
            _enrollmentRepository = enrollmenyRepository;
            _logger = logger;
        }

        public async Task<Response<Enrollment>> GetEnrollmentByAccountAsync(int accountId)
        {
            try
            {
                return await _enrollmentRepository.GetEnrollmentByEmailAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return  new Response<Enrollment> { Success = false , Message = "An error has occured" };
            }
        }
    }
}