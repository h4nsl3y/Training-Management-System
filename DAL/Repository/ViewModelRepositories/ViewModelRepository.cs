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
        public Result<T> GetTrainingEnrollmentView(int accountId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT * FROM TRAINING
                            INNER JOIN ENROLLMENT ON TRAINING.TRAININGID = ENROLLMENT.TRAININGID
                            LEFT OUTER JOIN TRAININGPREREQUISITE ON TRAININGPREREQUISITE.TRAININGID =  TRAINING.TRAININGID
                            WHERE ENROLLMENT.ACCOUNTID = @ACCOUNTID AND ENROLLMENT.STATEID = @STATEID;";
            parameters.Add(new SqlParameter("@ACCOUNTID", accountId));
            parameters.Add(new SqlParameter("@STATEID", EnrollmentStateEnum.Waiting_For_Approval));
            Result<T> result = _dataBaseUtil.ExecuteQuery(query,parameters);
            return result;
        }
    }
}
