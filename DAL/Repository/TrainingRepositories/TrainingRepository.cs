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

namespace DAL.Repository.TrainingRepositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDataBaseUtil<Training> _dataBaseUtil;
        private readonly string primaryKey;
        private readonly string tableName;
        public TrainingRepository(IDataBaseUtil<Training> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
            tableName = typeof(Training).Name;
            PropertyInfo[] properties = typeof(Training).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        public Result<Training> Getenrolled(int accountId)
        {
            string query =   @"SELECT * FROM TRAINING WHERE TRAININGID IN 
                             ( SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ACCOUNTID", accountId));
            return _dataBaseUtil.ExecuteQuery(query,parameters);
        }
        public Result<Training> GetUnenrolled(int accountId)
        { 
            string query =    @"SELECT * FROM TRAINING WHERE TRAININGID NOT IN 
                              ( SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ACCOUNTID", accountId));
            return _dataBaseUtil.ExecuteQuery(query, parameters);
        }
        public Result<bool> SetPrerequisite(int prerequisiteId, string title)
        {
            string query = @"INSERT INTO TRAININGPREREQUISITE (TRAININGID , PREREQUISITEID) VALUES ((SELECT TRAININGID FROM TRAINING WHERE TITLE = @TITLE) , @PREREQUISITEID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TITLE", title));
            parameters.Add(new SqlParameter("@PREREQUISITEID", prerequisiteId));
            return _dataBaseUtil.AffectedRows(query, parameters);
        }
    }
}