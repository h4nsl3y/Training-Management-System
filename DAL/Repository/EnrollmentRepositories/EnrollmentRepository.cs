﻿using DAL.DataBaseUtils;
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
        private readonly DataBaseUtil<Enrollment> _dataBaseUtil;
        public EnrollmentRepository(DataBaseUtil<Enrollment> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
        }
        public async Task<Response<Enrollment>> GetEnrollmentByEmailAsync(string email)
        {
            string query = @"SELECT * FROM ENROLLMENT 
                            WHERE ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL)
                            AND TRAININGID IN (SELECT TRAININGID FROM TRAINING WHERE STARTDATE > GETDATE()) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EMAIL", email) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<Enrollment>> GetEnrollmentIdByDeadline()
        {
            string query = @"SELECT * FROM ENROLLMENT WHERE TRAININGID IN
                                (SELECT TRAININGID FROM TRAINING WHERE DEADLINE = CAST(GETDATE() AS DATE))";
            return await _dataBaseUtil.ExecuteQueryAsync(query);
        }
        public async Task<Response<bool>> IsAnyEnrollment(int trainingId)
        {
            string query = @"SELECT * FROM ENROLLMENT WHERE TRAININGID = @TRAININGID ";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            Response<Enrollment> response = await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
            return new Response<bool> { Success = response.Success, Data = { response.Data.Count() > 0} };
        }
        public async Task SelectTrainingParticipants(Enrollment enrollment)
        {
            string query = $@"DELCARE @TRAININGCAPACITY;
                            SET @TRAINING = SELECT SEATNUMBER FROM TRAINING WHERE TRAININGID = @TRAININGID;
                            UPDATE ENROLLMENT
                            SET STATEID = CASE
                                    WHEN ENROLLMENT.ENROLLMENTID IN
				                            (
				                            SELECT TOP @TRAININGCAPACITY
				                            ENROLLMENTID FROM ENROLLMENT 
				                            JOIN TRAINING ON ENROLLMENT.TRAININGID = TRAINING.TRAININGID
				                            JOIN ACCOUNT ON ENROLLMENT.ACCOUNTID = ACCOUNT.ACCOUNTID
				                            WHERE ENROLLMENT.TRAININGID IN 
					                            (SELECT TRAININGID FROM TRAINING WHERE DEADLINE = CAST(GETDATE() AS DATE))
				                            AND STATEID = @ENROLLMENTSTATEAPPROVE
				                            ORDER BY 
				                            CASE
					                            WHEN TRAINING.DEPARTMENTID = ACCOUNT.ACCOUNTID
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
                new SqlParameter("@TRAININGID", enrollment.TrainingId) ,
                new SqlParameter("@ENROLLMENTSTATEAPPROVE", EnrollmentStateEnum.Confirmed),
                new SqlParameter("@ENROLLMENTSTATECONFIRM", EnrollmentStateEnum.Confirmed),
                new SqlParameter("@ENROLLMENTSTATEREJECT", EnrollmentStateEnum.Rejected)
            };
            await _dataBaseUtil.AffectedRowsAsync(query, parameters);
        }
    }
}
