using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RejectionBusinessLogics
{
    public interface IRejectionBusinessLogic
    {
        Task<Response<bool>> RegisterRejection(int enrollmentId, string employeeEmail, string comment);
    }
}
