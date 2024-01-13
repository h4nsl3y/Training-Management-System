using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.ApplicationProcessRepositories
{
    public interface IApplicationProcessRepository
    {
        Task<Response<AccountTraining>> GetAccountTraining();
        Task<Response<AccountTraining>> GetAccountTrainingByTraining(int trainingId);
    }
}
