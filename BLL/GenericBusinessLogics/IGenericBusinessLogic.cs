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
        Result<bool> Add(T entity);
        Result<bool> Delete(T entity);
        Result<T> Get(Dictionary<string, object> conditions);
        Result<T> GetAll(Dictionary<string, object> conditions = null);
        Result<bool> Update(int Id, Dictionary<string, object> conditions);
    }
}
