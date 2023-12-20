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
        Result<T> ExecuteQuery(string query, List<SqlParameter> parameters = null);
        Result<bool> AffectedRows(string query, List<SqlParameter> parameters = null);
    }
}
