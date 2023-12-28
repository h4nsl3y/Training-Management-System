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
        Task<Response<Training>> GetenrolledTrainingListAsync(int accountId);
        Task<Response<Training>> GetUnenrolleTrainingListdAsync(int accountId); 
        Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
    }
}
