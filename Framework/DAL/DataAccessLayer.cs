using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DAL
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly string _connString;
        private SqlConnection _connection;
        public DataAccessLayer()
        {
            _connString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public IEnumerable<T> ExecuteQuery<T>(string query, List<SqlParameter> parameters = null)
        {
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
                        T item = MapObject<T>(reader);
                        objectList.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return objectList;
        }
        public bool AffectedRows(string query, List<SqlParameter> parameters = null)
        {
            int rowsAffected = 0;
            try
            {
                Connect();
                using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
                {
                    if (parameters != null) { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception error)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return rowsAffected > 0;
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
        private T MapObject<T>(IDataReader reader)
        {
            Type type = typeof(T);
            T objectInstance = Activator.CreateInstance<T>();
            for (int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                string propertyName = reader.GetName(columnIndex);
                PropertyInfo property = typeof(T).GetProperty(propertyName ,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && reader.IsDBNull(columnIndex))
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
