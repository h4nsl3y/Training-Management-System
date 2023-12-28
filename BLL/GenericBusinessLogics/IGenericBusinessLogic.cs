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
        Task<Response<bool>> AddAsync(T entity);
        Task<Response<bool>> DeleteAsync(T entity);
        Task<Response<T>> GetAsync(Dictionary<string, object> conditions);
        Task<Response<T>> GetAllAsync(Dictionary<string, object> conditions = null);
        Task<Response<bool>> UpdateAsync(T entity);
    }
}
