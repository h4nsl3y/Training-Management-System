﻿using DAL.Entity;
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

namespace DAL.DataBaseHelpers
{
    public class DataBaseHelper<T> : IDataBaseHelper<T> where T : ISystemEntity
    {
        private readonly string _connString;
        private SqlConnection _connection;
        public DataBaseHelper()
        {
            _connString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        public async Task<Response<T>> ExecuteQueryAsync(string query, List<SqlParameter> parameters = null)
        {
            Response<T> result = new Response<T>();
            List<T> objectList = new List<T>();
            try
            {
                await ConnectAsync();
                using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
                {
                    if (parameters != null) { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        T entity = await Task.Run(() => MapObject(reader));
                        objectList.Add(entity);
                    }
                    reader.Close();
                    result.Success = true;
                    result.Data = objectList;
                    sqlCommand.Parameters.Clear();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
        public async Task<Response<bool>> ExecuteTransactionAffectedRowAsync(string query, List<SqlParameter> parameters = null)
        {
            Response<bool> result;
            try
            {
                await ConnectAsync();
                using (SqlTransaction transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, _connection, transaction))
                        {
                            if (parameters != null) { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                            int affectedrows = await sqlCommand.ExecuteNonQueryAsync();
                            transaction.Commit();
                            result = new Response<bool>() { Success = true, Data = { affectedrows > 0 } };
                        }
                    }
                    catch
                    {
                        transaction.Rollback(); 
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
        public async Task<Response<T>> ExecuteTransactionAsync(string query, List<SqlParameter> parameters = null)
        {
            Response<T> result = new Response<T>();
            List<T> objectList = new List<T>();
            try
            {
                await ConnectAsync();
                using (SqlTransaction transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(query, _connection, transaction))
                        {
                            if (parameters != null) { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                            while (reader.Read())
                            {
                                T entity = await Task.Run(() => MapObject(reader));
                                objectList.Add(entity);
                            }
                            reader.Close();
                            result.Success = true;
                            result.Data = objectList;
                            sqlCommand.Parameters.Clear();
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
        public async Task<Response<bool>> AffectedRowsAsync(string query, List<SqlParameter> parameters = null)
        {
            Response<bool> result;
            try
            {
                await ConnectAsync();
                using (SqlCommand sqlCommand = new SqlCommand(query, _connection))
                {
                    if (parameters != null) 
                    { sqlCommand.Parameters.AddRange(parameters.ToArray()); }
                    int affectedrows = await sqlCommand.ExecuteNonQueryAsync();
                    result = new Response<bool>() { Success = true, Data = { affectedrows > 0 } };
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return result;        
        }
        private async Task ConnectAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connString);
                await _connection.OpenAsync();
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
