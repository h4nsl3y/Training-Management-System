using DAL.DataBaseHelpers;
using DAL.Entity;
using DAL.Enum;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace DAL.Repository.AccountRepositories
{
    public class AccountRepository : RepositoryService, IAccountRepository
    {
        private readonly DataBaseHelper<Account> _dataBaseHelper;
        private readonly string tableName;
        public AccountRepository(DataBaseHelper<Account> dataBaseHelper)
        {
            _dataBaseHelper = dataBaseHelper;
            tableName = typeof(Account).Name;
        }
        public async Task<Response<Account>> AuthenticateAsync(string email)
        {
            string query = $@"SELECT PASSWORD FROM ACCOUNT WHERE EMAIL = @EMAIL";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter($"@EMAIL", email) };
            return await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<bool>> DuplicatedAsync(Dictionary<string, object> conditions)
        {
            Response<bool> duplicatedresult = new Response<bool>();
            foreach (var condition in conditions)
            {
                string query = $@"SELECT TOP  1 * FROM {tableName} 
                                  WHERE {condition.Key} = @{condition.Key}";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                Response<Account> queryResult = await _dataBaseHelper.ExecuteQueryAsync(query, parameters);
                duplicatedresult.Data.Add(queryResult.Data.Count > 0);
                duplicatedresult.Success = queryResult.Success;
            }
            return duplicatedresult;
        }
        public async Task<Response<Account>> GetAsync(Dictionary<string, object> conditions = null)
        {
            StringBuilder query = new StringBuilder(
                $@"SELECT ACCOUNT.ACCOUNTID,
                ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID,
                ACCOUNT.PASSWORD, ACCOUNTROLE.ROLEID
                FROM ACCOUNT
                INNER JOIN ACCOUNTROLE
                ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID
                WHERE ISACTIVE = 1");
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (conditions != null)
            {
                query.Append(" AND ");
                foreach (var condition in conditions)
                {
                    query.Append($"{condition.Key} = @{condition.Key} AND ");
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query.Length -= 5;
            }
            query.Append(" ;");
            Response<Account> queryResult = await _dataBaseHelper.ExecuteQueryAsync(query.ToString(), parameters);
            return new Response<Account>() { Success = true, Data = { queryResult.Data.FirstOrDefault() } };
        }
        public async Task<Response<Account>> GetAllAsync(Dictionary<string, object> conditions = null)
        {
            StringBuilder query = new StringBuilder(
                @"SELECT ACCOUNT.ACCOUNTID,
                ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                FROM ACCOUNT 
                INNER JOIN ACCOUNTROLE
                ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID
                WHERE ISACTIVE = 1");
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (conditions != null)
            {
                query.Append(" AND ");
                foreach (var condition in conditions)
                {
                    query.Append($"{condition.Key} = @{condition.Key} AND ");
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query.Length -= 5;
            }
            query.Append(" ;");
            return await _dataBaseHelper.ExecuteQueryAsync(query.ToString(), parameters);
        }
        public async Task<Response<Account>> GetActiveRequestEmployeeAsync(int managerId)
        {
            StringBuilder query = new StringBuilder(
                $@"SELECT 
                ACCOUNT.ACCOUNTID,
                ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                FROM ACCOUNT 
                INNER JOIN ACCOUNTROLE
                ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID 
                WHERE ACCOUNT.ISACTIVE = 1
                AND MANAGERID = @MANAGERID 
                AND ACCOUNT.ACCOUNTID
                IN (SELECT DISTINCT ACCOUNTID FROM ENROLLMENT WHERE STATEID = @STATEID )");

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter($"@MANAGERID", managerId),
                new SqlParameter($"@STATEID", EnrollmentStateEnum.Waiting_For_Approval )
            };
            return await _dataBaseHelper.ExecuteQueryAsync(query.ToString(),parameters);
        }
        public async Task<Response<Account>> GetManagerListAsync()
        {
            string query = $@"SELECT * FROM ACCOUNT INNER JOIN ACCOUNTROLE 
                              ON ACCOUNT.ACCOUNTID = ACCOUNTROLE.ACCOUNTID 
                              WHERE ACCOUNTROLE.ROLEID = @ROLEID1 OR ACCOUNTROLE.ROLEID = @ROLEID2";
            List<SqlParameter> parameters = new List<SqlParameter>() { 
                new SqlParameter($"@ROLEID1", RoleEnum.Manager), 
                new SqlParameter($"@ROLEID2", RoleEnum.Administrator)
            };
            return await _dataBaseHelper.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Response<Account>> RegisterAccountAsync(Account account)
        {
            string insertAccount = $@"DECLARE @ACCOUNTID INT;
                                    INSERT INTO ACCOUNT
                                    (FIRSTNAME, OTHERNAME, LASTNAME, NATIONALIDENTIFICATIONNUMBER, MOBILENUMBER, EMAIL, MANAGERID, DEPARTMENTID, PASSWORD)
                                    VALUES(@FIRSTNAME,@OTHERNAME,@LASTNAME,@NATIONALIDENTIFICATIONNUMBER,@MOBILENUMBER,@EMAIL,@MANAGERID,@DEPARTMENTID,@PASSWORD);
                                    SET @ACCOUNTID = SCOPE_IDENTITY(); 

                                    INSERT INTO ACCOUNTROLE(ACCOUNTID, ROLEID) VALUES(@ACCOUNTID, @ROLEID)

                                    SELECT * FROM ACCOUNT WHERE ACCOUNTID = @ACCOUNTID";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FIRSTNAME",GetPropertyValue(account.FirstName)),
                new SqlParameter("@OTHERNAME", GetPropertyValue(account.OtherName)),
                new SqlParameter("@LASTNAME", GetPropertyValue(account.LastName)),
                new SqlParameter("@NATIONALIDENTIFICATIONNUMBER", GetPropertyValue(account.NationalIdentificationNumber)),
                new SqlParameter("@MOBILENUMBER", GetPropertyValue(account.MobileNumber)),
                new SqlParameter("@EMAIL", GetPropertyValue(account.Email)),
                new SqlParameter("@MANAGERID", GetPropertyValue(account.ManagerId)),
                new SqlParameter("@DEPARTMENTID", GetPropertyValue(account.DepartmentId)),
                new SqlParameter("@PASSWORD", GetPropertyValue(account.Password)),
                new SqlParameter("@ROLEID", GetPropertyValue(account.RoleId))
            };
            return await Task.Run(() => _dataBaseHelper.ExecuteTransactionAsync(insertAccount, parameters));
        }
    }
}
