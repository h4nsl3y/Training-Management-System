using DAL.Entity;
using DAL.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBaseUtils
{
    public class DataBaseUtil<T> : IDataBaseUtil<T> where T : class
    {
        private readonly string _connString;
        private readonly ILogger _logger;
        private SqlConnection _connection;
        public DataBaseUtil(ILogger logger)
        {
            _logger = logger;
            _connString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public Result<T> ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            Result<T> result = new Result<T>();
            List<T> objectList = new List<T>();
            try
            {
                Connect();
                using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
                {
                    if (parameters != null) { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        T entity = MapObject(reader);
                        objectList.Add(entity);
                    }
                    reader.Close();
                    result.Success = true;
                    result.Data = objectList;
                    sqlCommand.Parameters.Clear();
                }
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                result.Success = false;
                result.Message = "No account has been find";
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
        public Result<bool> AffectedRows(string query, List<SqlParameter> parameters = null)
        {
            Result<bool> result = new Result<bool>();
            try
            {
                Connect();
                using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
                {
                    if (parameters != null) 
                    { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                    bool flag = sqlCommand.ExecuteNonQuery() > 0;
                    result.Data.Add(flag);
                    result.Success = true;
                }
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                result.Success = false;
                result.Message = "Server encountered an error";
            }
            finally
            {
                Disconnect();
            }
            return result;        
        }
        private void Connect()
        {
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    _connection = new SqlConnection(_connString);
                    _connection.Open();
                }
            }
            catch(Exception exception) 
            {
                _logger.Log(exception);
                throw;
            }
        }
        private void Disconnect()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        private T MapObject(IDataReader reader)
        {
            Type type = typeof(T);
            T objectInstance = Activator.CreateInstance<T>();
            for (int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                string propertyName = reader.GetName(columnIndex);
                PropertyInfo property = typeof(T).GetProperty(propertyName ,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && !reader.IsDBNull(columnIndex))
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    Object value = Convert.ChangeType(reader.GetValue(columnIndex), propertyType);
                    property.SetValue(objectInstance, value);
                }
            }
            return objectInstance;
        }
    }
}
