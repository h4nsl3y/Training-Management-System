using DAL.DataBaseUtils;
using DAL.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ISystemEntity
    {
        private readonly IDataBaseUtil<T> _dataBaseUtil;
        public GenericRepository(IDataBaseUtil<T> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
        }
        public Result<bool> Add(T entity)
        {
            string query = $"INSERT INTO {typeof(T).Name} (";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties.Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute))))
            {
                if (property.CanWrite) { query += $"{property.Name} ,"; }
            }
            query = query.Substring(0, query.Length - 1);
            query += ") VALUES (";
            foreach (PropertyInfo property in properties.Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute))))
            {
                if (property.CanWrite) { query += $"@{property.Name} ,"; }
                if (property.GetValue(entity) == null)
                { parameters.Add(new SqlParameter($"@{property.Name}", DBNull.Value)); }
                else
                { parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(entity))); }
            }
            query = query.Substring(0, query.Length - 1);
            query += ") ;";
            return _dataBaseUtil.AffectedRows(query, parameters);
        }
        public Result<bool> Delete(T entity)
        {
            string query = $"DELETE FROM  {typeof(T).Name} WHERE ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault();
            query += $"{primaryKey.Name} = @{primaryKey.Name}";
            parameters.Add(new SqlParameter($"@{primaryKey.Name}", primaryKey.GetValue(entity)));
            return _dataBaseUtil.AffectedRows(query, parameters);
        }
        public Result<T> Get(Dictionary<string, object> conditions)
        {
            Result<T> queryResult = GetData(conditions);
            Result<T> result = new Result<T>();
            result.Success = queryResult.Success;
            result.Data.Add(queryResult.Data.FirstOrDefault() );

            return result;
        }
        public Result<T> GetAll(Dictionary<string, object> conditions = null)
        {
            return (conditions==null) ? GetData() : GetData(conditions);
        }
        public Result<bool> Update(int Id, Dictionary<string, object> conditions)
        {
            string query = $"UPDATE {typeof(T).Name} SET ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault();
            
            foreach(var condition in conditions)
            {
                query += $"{condition.Key} = @{condition.Key} ,";
                parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
            }
            query = query.Substring(0, query.Length - 1);

            query += $" WHERE {primaryKey.Name} = @{primaryKey.Name}";
            parameters.Add(new SqlParameter($"@{primaryKey.Name}", Id));
            return _dataBaseUtil.AffectedRows(query, parameters);
        }
        private Result<T> GetData(Dictionary<string, object> conditions = null)
        {
            string query = "SELECT ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead) { query += $"{property.Name} ,"; }
            }
            query = query.Substring(0, query.Length - 1);
            query += $"FROM {typeof(T).Name} ";
            if (conditions != null)
            {
                query += " WHERE ";
                foreach (var condition in conditions)
                {
                    query += $"{condition.Key} = @{condition.Key} AND ";
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query = query.Substring(0, query.Length - 5);
            }
            query += " ;";
            return (conditions == null) ?
                _dataBaseUtil.ExecuteQuery(query) :
                _dataBaseUtil.ExecuteQuery(query, parameters);
        }
    }
}

