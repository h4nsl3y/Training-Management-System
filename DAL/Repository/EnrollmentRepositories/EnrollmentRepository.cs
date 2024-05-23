using DAL.DataBaseHelpers;
using DAL.Entity;
using DAL.Enum;
using DAL.Repository.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DAL.Repository.EnrollmentRepositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataBaseHelper<Enrollment> _dataBaseHelper;
        public EnrollmentRepository(DataBaseHelper<Enrollment> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
        }
        public async Task<Response<Enrollment>> GetEnrollmentByEmailAsync(int accountId)
        {
            string query = @"SELECT * FROM ENROLLMENT 
                            WHERE ACCOUNTID = @ACCOUNTID
                            AND TRAININGID IN (SELECT TRAININGID FROM TRAINING WHERE STARTDATE > GETDATE()) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ACCOUNTID", accountId) };
            return await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<Enrollment>> GetEnrollmentIdByDeadline()
        {
            string query = @"SELECT * FROM ENROLLMENT WHERE TRAININGID IN
                                (SELECT TRAININGID FROM TRAINING WHERE DEADLINE = CAST(GETDATE() AS DATE))";
            return await _dataBaseHelper.ExecuteQueryAsync(query);
        }
        public async Task<Response<Enrollment>> GetTrainingByDeadlineAsync()
        {
            string query = @"SELECT DISTINCT TRAININGID FROM ENROLLMENT WHERE TRAININGID IN 
                            (SELECT TRAININGID FROM TRAINING 
                            WHERE DEADLINE = CAST(GETDATE() AS DATE)) ;";
            return await _dataBaseHelper.ExecuteQueryAsync(query);
        }
        public async Task SelectTrainingParticipants(int trainingId)
        {
            string query = $@"UPDATE ENROLLMENT
                            SET STATEID = CASE
                                    WHEN ENROLLMENT.ENROLLMENTID IN
				                            (
				                            SELECT TOP (SELECT SEATNUMBER FROM TRAINING WHERE TRAININGID = @TRAININGID)
				                            ENROLLMENTID FROM ENROLLMENT 
				                            JOIN TRAINING ON ENROLLMENT.TRAININGID = TRAINING.TRAININGID
				                            JOIN ACCOUNT ON ENROLLMENT.ACCOUNTID = ACCOUNT.ACCOUNTID
				                            WHERE ENROLLMENT.TRAININGID = @TRAININGID
				                            AND (STATEID = @ENROLLMENTSTATEAPPROVE OR STATEID = @ENROLLMENTSTATECONFIRM)
				                            ORDER BY 
				                            CASE
					                            WHEN TRAINING.DEPARTMENTID = ACCOUNT.DEPARTMENTID 
					                            THEN 0
					                            ELSE 1
				                            END,
				                            SUBMISSIONDATE ASC
				                            )
                                        THEN @ENROLLMENTSTATECONFIRM
                                        ELSE @ENROLLMENTSTATEREJECT
                                    END
                            WHERE TRAININGID = @TRAININGID;";
            List<SqlParameter> parameters = new List<SqlParameter>() 
            { 
                new SqlParameter("@TRAININGID",  trainingId),
                new SqlParameter("@ENROLLMENTSTATEAPPROVE", EnrollmentStateEnum.Approved),
                new SqlParameter("@ENROLLMENTSTATECONFIRM", EnrollmentStateEnum.Confirmed),
                new SqlParameter("@ENROLLMENTSTATEREJECT", EnrollmentStateEnum.Rejected)
            };
            await _dataBaseHelper.AffectedRowsAsync(query, parameters);
        }
    }
}
