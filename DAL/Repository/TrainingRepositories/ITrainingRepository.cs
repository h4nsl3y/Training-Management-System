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
        Task<Result<Training>> GetenrolledTrainingListAsync(int accountId);
        Task<Result<Training>> GetUnenrolleTrainingListdAsync(int accountId); 
        Task<Result<bool>> SetPrerequisiteAsync(int prerequisiteId, string title);
    }
}
