﻿using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBaseHelpers
{
    public interface IDataBaseHelper<T>
    {
        Task<Response<T>> ExecuteQueryAsync(string query, List<SqlParameter> parameters = null);
        Task<Response<bool>> ExecuteTransactionAffectedRowAsync(string query, List<SqlParameter> parameters = null);
        Task<Response<T>> ExecuteTransactionAsync(string query, List<SqlParameter> parameters = null);
        Task<Response<bool>> AffectedRowsAsync(string query, List<SqlParameter> parameters = null);
    }
}
