using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBaseUtils
{
    public interface IDataBaseUtil<T>
    {
        Task<Response<T>> ExecuteQueryAsync(string query, List<SqlParameter> parameters = null);
        Task<Response<bool>> AffectedRowsAsync(string query, List<SqlParameter> parameters = null);
    }
}
