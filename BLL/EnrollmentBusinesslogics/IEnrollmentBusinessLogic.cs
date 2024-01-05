using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EnrollmentBusinesslogics
{
    public interface IEnrollmentBusinessLogic
    {
        Task<Response<Enrollment>> GetEnrollmentByEmailAsync(string email);
        Task<Response<bool>> IsAnyEnrollment(int trainingId);
    }
}
