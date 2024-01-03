using DAL.DataBaseUtils;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.RequiredFileRepositories
{
    public interface IRequiredFilesRepository
    {
        Task<Response<bool>> UpdateFileAsync(int prerequisiteId, int accountId, Dictionary<string, object> values);
        Task<Response<int>> CountFilePresentAsync(int trainingId, int accountId);
    }
}
