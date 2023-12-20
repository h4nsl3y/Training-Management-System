using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.ViewModelRepositories
{
    public interface IViewModelRepository<T>
    {
        Result<T> GetTrainingEnrollmentView(int accountId);
    }
}
