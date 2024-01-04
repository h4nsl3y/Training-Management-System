using DAL.DataBaseUtils;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.ViewModelRepositories
{
    public class ViewModelRepository<T> : IViewModelRepository<T>
    {
        private readonly IDataBaseUtil<T> _dataBaseUtil;
        public ViewModelRepository(IDataBaseUtil<T> dataBaseUtil) 
        { 
            _dataBaseUtil = dataBaseUtil;
        }
        public async Task<Response<T>> GetTrainingEnrollmentView(int accountId)
        {
            string query = $@"
                            WITH DATASET AS (
                            SELECT
                            TRAINING.TRAININGID, TRAINING.TITLE, TRAINING.DEPARTMENTID,
                            TRAINING.DEADLINE, TRAINING.STARTDATE, TRAINING.SHORTDESCRIPTION, 
                            ENROLLMENT.ENROLLMENTID, ENROLLMENT.ACCOUNTID, ENROLLMENT.STATEID, 
                            ENROLLMENT.SUBMISSIONDATE, TRAININGPREREQUISITE.PREREQUISITEID,
							ROW_NUMBER() over(PARTITION BY ENROLLMENT.ENROLLMENTID ORDER BY TRAINING.TRAININGID) as DUPLICATE
                            FROM TRAINING 
                            INNER JOIN ENROLLMENT ON TRAINING.TRAININGID = ENROLLMENT.TRAININGID
                            LEFT OUTER JOIN TRAININGPREREQUISITE ON TRAININGPREREQUISITE.TRAININGID =  TRAINING.TRAININGID
                            WHERE ENROLLMENT.ACCOUNTID = @ACCOUNTID AND ENROLLMENT.STATEID = @STATEID
							)
							SELECT TRAININGID, TITLE, DEPARTMENTID,
                            DEADLINE, STARTDATE, SHORTDESCRIPTION, 
                            ENROLLMENTID, ACCOUNTID, STATEID, 
                            SUBMISSIONDATE, PREREQUISITEID
							FROM DATASET
							WHERE DUPLICATE =1";
            List<SqlParameter> parameters = new List<SqlParameter>() 
            {
                new SqlParameter("@ACCOUNTID", accountId),
                new SqlParameter("@STATEID", EnrollmentStateEnum.Waiting_For_Approval)
            };
            Response<T> result = await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
            return result;
        }
    }
}