using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.TrainingBusinessLogics
{
    public interface ITrainingBusinessLogic
    {
        Task<Response<bool>> DeleteTrainingAsync(int trainingId);
        Task<Response<Training>> GetAllTrainingAsync();
        Task<Response<Training>> GetTrainingAsync(int trainingId);
        Task<Response<Training>> GetEnrolledTrainingAsync(int accountId);
        Task<Response<Training>> GetAvailableTrainingAsync(int accountId);
        Task<Response<bool>> IsAnyEnrollmentByTrainingAsync(int trainingId);
        Task<Response<bool>> RegisterTrainingAsync(Training training, List<int> prerequisites);
        Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
        Task<Response<bool>> UpdateTrainingAsync(Training training, List<int> prerequisites);
    }
}
