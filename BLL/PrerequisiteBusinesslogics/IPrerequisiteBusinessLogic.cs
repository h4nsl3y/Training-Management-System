using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.PrerequisiteBusinesslogics
{
    public interface IPrerequisiteBusinessLogic
    {
        Task<Response<Prerequisite>> GetPrequisiteAsync(int trainingid);
        Task<Response<int>> GetPrerequisiteIdByEmployee(int accountId);
    }
}
