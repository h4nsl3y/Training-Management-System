using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.PrerequisiteRepositories
{
    public interface IPrerequisiteRepository
    {
        Task<Response<Prerequisite>> GetPrequisiteAsync(int trainingId);
        Task<Response<int>> GetPrerequisiteIdByEmployee(int accountId);
    }
}
