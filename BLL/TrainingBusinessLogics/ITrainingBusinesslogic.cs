using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.TrainingBusinessLogics
{
    public interface ITrainingBusinesslogic
    {
        Task<Response<Training>> GetEnrolledAsync(int accountId);
        Task<Response<Training>> GetUnenrolledAsync(int accountId);
        Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
    }
}
