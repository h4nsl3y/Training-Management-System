using DAL.DataBaseUtils;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL.Repository.EnrollmentRepositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataBaseUtil<Enrollment> _dataBaseUtil;
        public EnrollmentRepository(DataBaseUtil<Enrollment> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
        }

        public async Task<Response<Enrollment>> GetEnrollmentByEmailAsync(string email)
        {
            string query = @"SELECT * FROM ENROLLMENT 
                            WHERE ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL)
                            AND TRAININGID IN (SELECT TRAININGID FROM TRAINING WHERE DEADLINE > GETDATE()) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EMAIL", email) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }

        public async Task<Response<Enrollment>> GetAllEnrollmentByEmail(string email)
        {
            string query = @"SELECT * FROM ENROLLMENT 
                             WHERE ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL)
                            AND TRAININGID IN (SELECT TRAININGID FROM TRAINING WHERE DEADLINE > GETDATE()) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EMAIL", email) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
    }
}