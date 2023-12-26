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
        public async Task<Result<Prerequisite>> GetPrequisiteAsync(int trainingId)
        {
            string query = @"SELECT * FROM PREREQUISITE 
                             WHERE PREREQUISITEID IN 
                                (SELECT PREREQUISITEID FROM TrainingPrerequisite 
                                WHERE TRAININGID = @TRAININGID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Result<int>> GetPrerequisiteIdByEmployee(int accountId)
        {
            
            string query = @"SELECT * FROM PREREQUISITE 
                             WHERE PREREQUISITEID IN 
                                (SELECT PREREQUISITEID FROM REQUIREDFILES 
                                WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ACCOUNTID", accountId) };
            Result<Prerequisite> prerequisiteResult = await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
            return new Result<int>() { 
                Success = true ,
                Data = prerequisiteResult.Data.Select(prerequisites => prerequisites.PrerequisiteId).ToList()
             };
        }
    }
}
