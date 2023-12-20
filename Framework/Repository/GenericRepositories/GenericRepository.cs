using Framework.Custom;
using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.GenericRepositories
{
    public class GenericRepository<T> : IGenericRepository<T>
    {
        private readonly IDataAccessLayer _dataAccessLayer;
        public GenericRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }
        public bool Delete(T item)
        {
            string query = $"DELETE FROM  {typeof(T).Name} WHERE ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo primaryKey = properties.Where(p => !Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))).FirstOrDefault();
            query += $"{primaryKey.Name} = @{primaryKey.Name}";
            parameters.Add(new SqlParameter($"@{primaryKey.Name}", primaryKey.GetValue(item)));
            return _dataAccessLayer.AffectedRows(query, parameters);
        }
        public T Get(Dictionary<string, object> conditions)
        {
            return GetData(conditions).FirstOrDefault();
        }
        public IEnumerable<T> GetAll()
        {
            return GetData();
        }
        public bool Insert(T item)
        {
            string query = $"INSERT INTO {typeof(T).Name} (";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties.Where(p => !Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))))
            {
                if (property.CanWrite) { query += $"{property.Name} ,"; }
            }
            query += ") VALUES";
            foreach (PropertyInfo property in properties.Where(p => !Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))))
            {
                if (property.CanWrite) { query += $"@{property.Name} ,"; }
                parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(item)));
            }
            query += " ;";
            return _dataAccessLayer.AffectedRows(query, parameters);
        }
        public bool Update(T item, Dictionary<string, object> conditions)
        {
            string query = $"UPDATE {typeof(T).Name} SET ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo primaryKey = properties.Where(p => !Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))).FirstOrDefault();
            foreach (PropertyInfo property in properties.Where(p => !Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))))
            {
                if (property.CanRead) { query += $"{property.Name} = @{property.Name} ,"; }
                parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(item)));
            }
            query.Remove(query.Length - 1, 1);
            if (primaryKey != null)
            {
                query += $"WHERE {primaryKey.Name} = @{primaryKey} ;";
                parameters.Add(new SqlParameter($"@{primaryKey.Name}", primaryKey.GetValue(item)));
            }
            return _dataAccessLayer.AffectedRows(query, parameters);
        }
        private IEnumerable<T> GetData(Dictionary<string, object> conditions = null)
        {
            string query = "SELECT ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead) { query += $"{property.Name} ,"; }
            }
            query.Remove(query.Length - 1, 1);
            query += $"FROM {typeof(T).Name} ";
            if (conditions != null)
            {
                query += " WHERE ";
                foreach (var condition in conditions)
                {
                    query += $"{condition.Key} = @{condition.Key} AND ";
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query.Remove(query.Length - 5, 5);
            }
            query += " ;";
            return (conditions == null) ?
                _dataAccessLayer.ExecuteQuery<T>(query) :
                _dataAccessLayer.ExecuteQuery<T>(query, parameters);
        }
    }
}
