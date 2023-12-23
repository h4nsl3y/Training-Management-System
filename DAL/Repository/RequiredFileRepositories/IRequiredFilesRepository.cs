using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.RequiredFileRepositories
{
    public interface IRequiredFilesRepository
    {
        Result<bool> UpdateFile(int prerequisiteId, int accountId, Dictionary<string, object> values);
    }
}
