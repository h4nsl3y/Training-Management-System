using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DAL
{
    public interface IDataAccessLayer
    {
        IEnumerable<T> ExecuteQuery<T>(string query, List<SqlParameter> parameters = null);
        bool AffectedRows(string query, List<SqlParameter> parameters = null);
    }
}
