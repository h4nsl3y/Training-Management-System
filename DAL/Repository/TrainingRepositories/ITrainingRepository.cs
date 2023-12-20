using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.TrainingRepositories
{
    public interface ITrainingRepository
    {
        Result<Training> Getenrolled(int accountId);
        Result<Training> GetUnenrolled(int accountId);
    }
}
