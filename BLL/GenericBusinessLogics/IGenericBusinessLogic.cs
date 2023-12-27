using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.GenericBusinessLogics
{
    public interface IGenericBusinessLogic<T> 
    {
        Task<Result<bool>> AddAsync(T entity);
        Task<Result<bool>> DeleteAsync(T entity);
        Task<Result<T>> GetAsync(Dictionary<string, object> conditions);
        Task<Result<T>> GetAllAsync(Dictionary<string, object> conditions = null);
        Task<Result<bool>> UpdateAsync(int Id, Dictionary<string, object> conditions);
    }
}
