using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.TrainingRepositories
{
    public interface ITrainingRepository
    {
        Task<Response<bool>> DeleteTrainingAsync(int trainingId);
        Task<Response<Training>> GetAllTrainingAsync();
        Task<Response<Training>> GetTrainingAsync(int trainingId);
        Task<Response<Training>> GetEnrolledTrainingAsync(int accountId);
        Task<Response<Training>> GetUnenrolleTrainingAsync(int accountId);
        Task<Response<bool>> RegisterTrainingAsync(Training training, List<int> prerequisites);
        Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
        Task<Response<bool>> UpdateTrainingAsync(Training training, List<int> prerequisites);
    }
}
