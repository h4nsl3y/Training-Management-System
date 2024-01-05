using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EnrollmentRepositories
{
    public interface IEnrollmentRepository
    {
        Task<Response<Enrollment>> GetEnrollmentByEmailAsync(string email);
        Task<Response<Enrollment>> GetEnrollmentIdByDeadline();
        Task<Response<bool>> IsAnyEnrollment(int trainingId);
        Task SelectTrainingParticipants(Enrollment enrollment);
    }
}
