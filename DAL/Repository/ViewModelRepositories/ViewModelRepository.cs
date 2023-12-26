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
        public async Task<Result<T>> GetTrainingEnrollmentView(int accountId)
        {
            string query = $@"SELECT * FROM TRAINING
                            INNER JOIN ENROLLMENT ON TRAINING.TRAININGID = ENROLLMENT.TRAININGID
                            LEFT OUTER JOIN TRAININGPREREQUISITE ON TRAININGPREREQUISITE.TRAININGID =  TRAINING.TRAININGID
                            WHERE ENROLLMENT.ACCOUNTID = @ACCOUNTID AND ENROLLMENT.STATEID = @STATEID;";

            List<SqlParameter> parameters = new List<SqlParameter>() 
            {
                new SqlParameter("@ACCOUNTID", accountId),
                new SqlParameter("@STATEID", EnrollmentStateEnum.Waiting_For_Approval)
            };
            Result<T> result = await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
            return result;
        }
    }
}
