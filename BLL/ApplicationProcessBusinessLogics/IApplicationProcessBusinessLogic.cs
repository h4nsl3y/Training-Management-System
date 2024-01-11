using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AccountTrainingBusinessLogics
{
    public interface IApplicationProcessBusinessLogic
    {
        Task<Response<byte[]>> CreateExcelFile(int trainingId);
        void SendEmail(Enrollment enrollment, string trainingTitle, string recipientEmail, string comment = null);
    }
}
