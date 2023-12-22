using DAL.DataBaseUtils;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.PrerequisiteRepositories
{
    public class PrerequisiteRepository : IPrerequisiteRepository
    {
        private readonly IDataBaseUtil<Prerequisite> _dataBaseUtil;
        private readonly string primaryKey;
        private readonly string tableName;

        public PrerequisiteRepository(IDataBaseUtil<Prerequisite> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
            PropertyInfo[] properties = typeof(Prerequisite).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
            tableName = typeof(Prerequisite).Name;
        }
        public Result<Prerequisite> GetPrequisite(int trainingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT * FROM PREREQUISITE WHERE PREREQUISITEID IN (SELECT PREREQUISITEID FROM TrainingPrerequisite WHERE TRAININGID = @TRAININGID) ;";
            parameters.Add(new SqlParameter("@TRAININGID", trainingId));
            return _dataBaseUtil.ExecuteQuery(query, parameters);
        }
        public Result<int> GetPrerequisiteIdByEmployee(int accountId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT * FROM PREREQUISITE WHERE PREREQUISITEID IN (SELECT PREREQUISITEID FROM REQUIREDFILES WHERE ACCOUNTID = @ACCOUNTID) ;";
            parameters.Add(new SqlParameter("@ACCOUNTID", accountId));
            Result<Prerequisite> prerequisiteResult = _dataBaseUtil.ExecuteQuery(query, parameters);
            Result<int> result = new Result<int>();
            result.Data = prerequisiteResult.Data.Select(prerequisites => prerequisites.PrerequisiteId).ToList();
            result.Success = true;
            return result;  
        }
    }
}
