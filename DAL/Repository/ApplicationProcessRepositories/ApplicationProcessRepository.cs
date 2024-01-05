using DAL.DataBaseUtils;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.ApplicationProcessRepositories
{
    public class ApplicationProcessRepository : IApplicationProcessRepository
    {
        private readonly IDataBaseUtil<AccountTraining> _dataBaseUtil;
        public ApplicationProcessRepository(IDataBaseUtil<AccountTraining> dataBaseUtil) 
        {
            _dataBaseUtil = dataBaseUtil;
        }
        public async Task<Response<AccountTraining>> GetAccountTrainingData(int trainingId)
        {
            string query = @"SELECT CONCAT(A1.FIRSTNAME,' ',A1.OTHERNAME,' ',A1.LASTNAME) AS USERNAME,
                                    A1.MOBILENUMBER,
                                    A1.EMAIL,
                                    CONCAT(A2.FIRSTNAME,' ',A2.OTHERNAME,' ',A2.LASTNAME) AS MANAGERNAME,
                                    T.TITLE,
                                    T.STARTDATE 
                            FROM ACCOUNT A1
                            JOIN ACCOUNT A2 ON A1.MANAGERID = A2.ACCOUNTID
                            JOIN ENROLLMENT E ON E.ACCOUNTID = A1.ACCOUNTID
                            JOIN TRAINING T ON T.TRAININGID = E.TRAININGID
                            WHERE T.TRAININGID = @TRAININGID
                            AND E.STATEID = @STATEID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TRAININGID",trainingId),
                 new SqlParameter("@STATEID",EnrollmentStateEnum.Confirmed)
            };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
    }
}
