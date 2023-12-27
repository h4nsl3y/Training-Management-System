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
        Task<Result<Training>> GetEnrolledAsync(int accountId);
        Task<Result<Training>> GetUnenrolledAsync(int accountId);
        Task<Result<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
    }
}
