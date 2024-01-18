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
        Task<Response<bool>> AddEnrollmentAsync(Enrollment enrollment, string trainingTitle, string email, string comment);
        Task<Response<Enrollment>> GetEnrollmentByAccountAsync(int accountId);
        Task<Response<byte[]>> CreateExcelFile(int trainingId);
        Task RenewSelection(Enrollment enrollment);
    }
}
