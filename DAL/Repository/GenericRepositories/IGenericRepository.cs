using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.GenericRepositories
{
    public interface IGenericRepository<T>
    {
        Task<Response<bool>> AddAsync(T entity);
        Task<Response<bool>> DeleteAsync(T entity);
        Task<Response<T>> GetAsync(Dictionary<string, object> conditions);
        Task<Response<T>> GetAllAsync(Dictionary<string, object> conditions = null);
        Task<Response<bool>> UpdateAsync(T entity);//int Id, Dictionary<string, object> conditions);
    }
}
