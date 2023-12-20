using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModelsBusinessLogics
{
    public interface IViewModelBusinesslogic<T>
    {
        Result<T> GetTrainingEnrollmentView(int accountId);
    }
}
