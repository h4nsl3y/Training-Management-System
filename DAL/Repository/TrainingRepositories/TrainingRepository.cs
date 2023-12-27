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
        public TrainingRepository(IDataBaseUtil<Training> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
        }
        public async Task<Result<Training>> GetenrolledTrainingListAsync(int accountId)
        {
            string query =   @"SELECT * FROM TRAINING WHERE 
                               ENDDATE > GETDATE() AND
                                TRAININGID IN 
                             ( SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID ) ;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ACCOUNTID", accountId)
            };
            return await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Result<Training>> GetUnenrolleTrainingListdAsync(int accountId)
        { 
            string query =  @"SELECT * FROM TRAINING WHERE 
                              DEADLINE > GETDATE() AND
                              TRAININGID NOT IN 
                             (SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ACCOUNTID", accountId)
            };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Result<bool>> SetPrerequisiteAsync(int prerequisiteId, string title)
        {
            string query = @"INSERT INTO TRAININGPREREQUISITE (TRAININGID , PREREQUISITEID) 
                            VALUES ((SELECT TRAININGID FROM TRAINING
                            WHERE TITLE = @TITLE) , @PREREQUISITEID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TITLE", title));
            parameters.Add(new SqlParameter("@PREREQUISITEID", prerequisiteId));
            return await _dataBaseUtil.AffectedRowsAsync(query, parameters);
        }
    }
}