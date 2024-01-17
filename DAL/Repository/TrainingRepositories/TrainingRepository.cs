using DAL.DataBaseHelpers;
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
        private readonly IDataBaseHelper<Training> _dataBaseHelper;
        public TrainingRepository(IDataBaseHelper<Training> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
        }
        public async Task<Response<bool>> DeleteTrainingAsync(int trainingId)
        {
            string query = @"UPDATE TRAINING SET ISACTIVE = 0 WHERE TRAININGID = @TRAININGID;
                            DELETE FROM TRAININGPREREQUISITE WHERE TRAININGID = @TRAININGID;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            return await _dataBaseHelper.ExecuteTransactionAffectedRowAsync(query, parameters);
        }
        public async Task<Response<Training>> GetAllTrainingAsync()
        {
            string query = @"SELECT * FROM TRAINING WHERE ISACTIVE = 1;";
            return await _dataBaseHelper.ExecuteQueryAsync(query);
        }
        public async Task<Response<Training>> GetTrainingAsync(int trainingId)
        {
            string query = @"SELECT TOP 1 * FROM TRAINING WHERE ISACTIVE = 1 AND TRAININGID = @TRAININGID;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            return await _dataBaseHelper.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Response<Training>> GetEnrolledTrainingAsync(int accountId)
        {
            string query = @"SELECT * FROM TRAINING WHERE 
                            ISACTIVE = 1 AND
                            ENDDATE > GETDATE() AND
                            TRAININGID IN ( SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID ) ;";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ACCOUNTID", accountId) };
            return await _dataBaseHelper.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Response<Training>> GetAvailableTrainingAsync(int accountId)
        { 
            string query = @"SELECT * FROM TRAINING WHERE 
                            ISACTIVE = 1 AND
                            DEADLINE > GETDATE() AND
                            TRAININGID NOT IN 
                            (SELECT TRAININGID FROM ENROLLMENT WHERE ACCOUNTID = @ACCOUNTID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ACCOUNTID", accountId)
            };
            return await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<Training>> GetTrainingByDeadlineAsync()
        {
            string query = @"SELECT TRAININGID FROM TRAINING 
                            WHERE ISACTIVE = 1 
                            AND DEADLINE = CAST(GETDATE() AS DATE) ;";
            return await _dataBaseHelper.ExecuteQueryAsync(query);
        }
        public async Task<Response<bool>> IsAnyEnrollmentByTrainingAsync(int trainingId)
        {
            string query = @"SELECT TOP 1 * FROM ENROLLMENT 
                            WHERE TRAININGID = @TRAININGID 
                            AND STATEID NOT IN 
                                (SELECT STATEID FROM STATE 
                                WHERE STATEDEFINITION = 'CANCELLED' 
                                OR STATEDEFINITION = 'REJECTED') ; ";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@TRAININGID", trainingId) };
            Response<Training> response = await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
            return new Response<bool> { Success = response.Success, Data = { response.Data.Count() > 0 } };
        }
        public async Task<Response<bool>> RegisterTrainingAsync(Training training, List<int> prerequisites)
        {
            string prerequisiteList = String.Join(",", prerequisites);
            string query = @"DECLARE @TRAININGID INT;
                        INSERT INTO TRAINING (TITLE, DEPARTMENTID, SEATNUMBER, DEADLINE, STARTDATE, ENDDATE, SHORTDESCRIPTION, LONGDESCRIPTION)
                        VALUES (@TITLE, @DEPARTMENTID, @SEATNUMBER, @DEADLINE, @STARTDATE, @ENDDATE, @SHORTDESCRIPTION, @LONGDESCRIPTION)
                        SET @TRAININGID = SCOPE_IDENTITY(); 
                        IF (@PREREQUISITEIDS IS NOT NULL AND LEN(@PREREQUISITEIDS) > 0)
                        BEGIN
                            INSERT INTO TRAININGPREREQUISITE (TRAININGID, PREREQUISITEID)
                            SELECT @TrainingId, P.value
                            FROM STRING_SPLIT(@PREREQUISITEIDS, ',') AS P;
                        END";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TITLE", training.Title),
                new SqlParameter("@DEPARTMENTID", training.DepartmentId),
                new SqlParameter("@SEATNUMBER", training.SeatNumber),
                new SqlParameter("@DEADLINE", training.Deadline),
                new SqlParameter("@STARTDATE", training.StartDate),
                new SqlParameter("@ENDDATE", training.EndDate),
                new SqlParameter("@SHORTDESCRIPTION", training.ShortDescription),
                new SqlParameter("@LONGDESCRIPTION", training.LongDescription),
                new SqlParameter("@PREREQUISITEIDS", prerequisiteList)
            };
            return await _dataBaseHelper.ExecuteTransactionAffectedRowAsync(query, parameters);
        }
        public async Task<Response<bool>> SetPrerequisiteAsync(int prerequisiteId, string title)
        {
            string query = @"INSERT INTO TRAININGPREREQUISITE (TRAININGID , PREREQUISITEID) 
                            VALUES ((SELECT TRAININGID FROM TRAINING
                            WHERE TITLE = @TITLE) , @PREREQUISITEID) ;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TITLE", title),
                new SqlParameter("@PREREQUISITEID", prerequisiteId)
            };
            return await _dataBaseHelper.AffectedRowsAsync(query, parameters);
        }
        public async Task<Response<bool>> UpdateTrainingAsync(Training training, List<int> prerequisites)
        {
            string prerequisiteList = String.Join(",", prerequisites);
            string query = @"
                        UPDATE TRAINING SET 
                        TITLE = @TITLE, 
                        DEPARTMENTID = @DEPARTMENTID, 
                        SEATNUMBER = @SEATNUMBER, 
                        DEADLINE =  @DEADLINE, 
                        STARTDATE = @STARTDATE, 
                        ENDDATE = @ENDDATE, 
                        SHORTDESCRIPTION = @SHORTDESCRIPTION, 
                        LONGDESCRIPTION = @LONGDESCRIPTION
                        WHERE TRAININGID = @TRAININGID;
                        DELETE FROM TRAININGPREREQUISITE WHERE TRAININGID = @TRAININGID;
                        IF (@PREREQUISITEIDS IS NOT NULL AND LEN(@PREREQUISITEIDS) > 0)
                        BEGIN
                            INSERT INTO TRAININGPREREQUISITE (TRAININGID, PREREQUISITEID)
                            SELECT @TrainingId, P.value
                            FROM STRING_SPLIT(@PREREQUISITEIDS, ',') AS P;
                        END;";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TRAININGID", training.TrainingId),
                new SqlParameter("@TITLE", training.Title),
                new SqlParameter("@DEPARTMENTID", training.DepartmentId),
                new SqlParameter("@SEATNUMBER", training.SeatNumber),
                new SqlParameter("@DEADLINE", training.Deadline),
                new SqlParameter("@STARTDATE", training.StartDate),
                new SqlParameter("@ENDDATE", training.EndDate),
                new SqlParameter("@SHORTDESCRIPTION", training.ShortDescription),
                new SqlParameter("@LONGDESCRIPTION", training.LongDescription),
                new SqlParameter("@PREREQUISITEIDS", prerequisiteList)
            };
            return await _dataBaseHelper.ExecuteTransactionAffectedRowAsync(query, parameters);
        }
    }
}