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
        private readonly IEnrollmentRepository _enrollmenyRepository;
        private readonly ILogger _logger;
        public EnrollmentBusinesslogic(IEnrollmentRepository enrollmenyRepository, ILogger logger)
        {
            _enrollmenyRepository = enrollmenyRepository;
            _logger = logger;
        }

        public async Task<Result<Enrollment>> GetEnrollmentByEmailAsync(string email)
        {
            try
            {
                return await _enrollmenyRepository.GetEnrollmentByEmailAsync(email);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return  new Result<Enrollment> { Success = false , Message = "An Error" };
            }
        }
    }
}