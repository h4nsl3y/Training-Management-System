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
        bool SendEmail(string Subject, string Body, string recipientEmail);
    }
}
