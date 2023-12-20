using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.GenericRepositories
{
    public interface IGenericRepository<T>
    {
        T Get(Dictionary<string, object> conditions);
        IEnumerable<T> GetAll();
        bool Insert(T item);
        bool Update(T item, Dictionary<string, object> conditions);
        bool Delete(T item);
    }
}
