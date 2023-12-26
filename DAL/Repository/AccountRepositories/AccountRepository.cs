using DAL.DataBaseUtils;
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
    public class AccountRepository : IAccountRepository
    {
        private readonly DataBaseUtil<Account> _dataBaseUtil;
        private readonly string primaryKey;
        private readonly string tableName;
        public AccountRepository(DataBaseUtil<Account> dataBaseUtil)
        {
            _dataBaseUtil = dataBaseUtil;
            PropertyInfo[] properties = typeof(Account).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
            tableName = typeof(Account).Name;
        }
        public async Task<Result<bool>> AddAsync (Account account)
        {
            string insertAccount = $@"INSERT INTO {tableName}
                                     (FIRSTNAME, OTHERNAME, LASTNAME, NATIONALIDENTIFICATIONNUMBER, MOBILENUMBER, EMAIL, MANAGERID, DEPARTMENTID, PASSWORD)
                               VALUES(@FIRSTNAME,@OTHERNAME,@LASTNAME,@NATIONALIDENTIFICATIONNUMBER,@MOBILENUMBER,@EMAIL,@MANAGERID,@DEPARTMENTID,@PASSWORD);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@FIRSTNAME",IsDataNull(account.FirstName)),
                new SqlParameter("@OTHERNAME", IsDataNull(account.OtherName)),
                new SqlParameter("@LASTNAME", IsDataNull(account.LastName)),
                new SqlParameter("@NATIONALIDENTIFICATIONNUMBER", IsDataNull(account.NationalIdentificationNumber)),
                new SqlParameter("@MOBILENUMBER", IsDataNull(account.MobileNumber)),
                new SqlParameter("@EMAIL", IsDataNull(account.Email)),
                new SqlParameter("@MANAGERID", IsDataNull(account.ManagerId)),
                new SqlParameter("@DEPARTMENTID", IsDataNull(account.DepartmentId)),
                new SqlParameter("@PASSWORD", IsDataNull(account.Password))
        };
            
           
            Result<bool> insertAccountResult = await Task.Run(() => _dataBaseUtil.AffectedRowsAsync(insertAccount, parameters));
            Result<bool> insertRoleResult = await Task.Run(() => SetRoleAsync(account.Email, account.RoleId));

            return (insertAccountResult.Success == true && insertRoleResult.Success == true) ?
                new Result<bool>() { Data = { true }, Success = true } :
                new Result<bool>() { Data = { false }, Success = false };
        }
        public async Task<Result<Account>> AuthenticateAsync(string email)
        {
            string query = $@"SELECT PASSWORD FROM ACCOUNT WHERE EMAIL = @EMAIL";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter($"@EMAIL", email) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Result<bool>> DuplicatedAsync(Dictionary<string, object> conditions)
        {
            Result<bool> duplicatedresult = new Result<bool>();
            foreach (var condition in conditions)
            {
                string query = $@"SELECT TOP  1 * FROM {tableName} 
                                  WHERE {condition.Key} = @{condition.Key} ;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                Result<Account> queryResult = await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
                duplicatedresult.Data.Add(queryResult.Data.Count > 0);
                duplicatedresult.Success = queryResult.Success;
            }
            return duplicatedresult;
        }
        public async Task<Result<Account>> GetAsync(Dictionary<string, object> conditions = null)
        {
            string query = $@"SELECT ACCOUNT.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID,
                            ACCOUNT.PASSWORD, ACCOUNTROLE.ROLEID
                            FROM ACCOUNT
                            INNER JOIN ACCOUNTROLE
                            ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (conditions != null)
            {
                query += " WHERE ";
                foreach (var condition in conditions)
                {
                    query += $"{condition.Key} = @{condition.Key} AND ";
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query = query.Substring(0, query.Length - 5);
            }
            query += " ;";
            Result<Account> queryResult =  (conditions == null) ?
                                        await _dataBaseUtil.ExecuteQueryAsync(query) :
                                        await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
            return new Result<Account>() { Success = true, Data = { queryResult.Data.FirstOrDefault() } };
        }
        public async Task<Result<Account>> GetAllAsync(Dictionary<string, object> conditions = null)
        {
            string query = $@"SELECT ACCOUNT.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                            FROM ACCOUNT 
                            INNER JOIN ACCOUNTROLE
                            ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (conditions != null)
            {
                query += " WHERE ";
                foreach (var condition in conditions)
                {
                    query += $"{condition.Key} = @{condition.Key} AND ";
                    parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                }
                query = query.Substring(0, query.Length - 5);
            }
            query += " ;";
            return (conditions == null) ?
                await _dataBaseUtil.ExecuteQueryAsync(query) :
                await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Result<Account>> GetActiveRequestEmployeeAsync(int managerId)
        {
            string query = $@"SELECT 
                            ACCOUNT.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                            FROM ACCOUNT 
                            INNER JOIN ACCOUNTROLE
                            ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID 
                            WHERE MANAGERID = @MANAGERID AND ACCOUNT.ACCOUNTID
                            IN (SELECT DISTINCT ACCOUNTID FROM ENROLLMENT WHERE STATEID = @STATEID )";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter($"@MANAGERID", managerId),
                new SqlParameter($"@STATEID", EnrollmentStateEnum.Waiting_For_Approval )
            };
            return await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Result<Account>> GetLastRegisteredAccountAsync()
        {
            string query = $@"SELECT TOP 1 
                            ACCOUNT.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                            FROM {tableName} 
                            INNER JOIN ACCOUNTROLE
                            ON ACCOUNTROLE.ACCOUNTID = ACCOUNT.ACCOUNTID
                            ORDER BY {primaryKey} DESC; ";
            Result<Account> result = await _dataBaseUtil.ExecuteQueryAsync(query);
            result.Data = new List<Account>() { result.Data.FirstOrDefault() }; 
            return result;
        }
        public async Task<Result<Account>> GetManagerListAsync()
        {
            
            string query = $@"SELECT *FROM ACCOUNT INNER JOIN ACCOUNTROLE 
                              ON ACCOUNT.ACCOUNTID = ACCOUNTROLE.ACCOUNTID WHERE ACCOUNTROLE.ROLEID = @ROLEID";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter($"@ROLEID", RoleEnum.Manager) };
            return await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Result<bool>> SetRoleAsync(string email, int roleId)
        {
            string query = @"INSERT INTO ACCOUNTROLE(ACCOUNTID, ROLEID) VALUES((SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL), @ROLEID);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EMAIL", email),
                new SqlParameter("@ROLEID", roleId)
            };
            return await Task.Run(() => _dataBaseUtil.AffectedRowsAsync(query, parameters));
        }

        private object IsDataNull(object value) => (value == null) ? DBNull.Value : value;
    }
}
