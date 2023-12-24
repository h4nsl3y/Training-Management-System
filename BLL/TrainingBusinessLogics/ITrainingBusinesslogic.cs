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
        Result<Training> GetEnrolled(int accountId);
        Result<Training> GetUnenrolled(int accountId);
        Result<bool> SetPrerequisite(int prerequisiteId, string title);
    }
}
