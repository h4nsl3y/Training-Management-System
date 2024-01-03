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
    public class AccountRepository : RepositoryService, IAccountRepository
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
        public async Task<Response<bool>> AddAsync (Account account)
        {
            string insertAccount = $@"INSERT INTO {tableName}
                                     (FIRSTNAME, OTHERNAME, LASTNAME, NATIONALIDENTIFICATIONNUMBER, MOBILENUMBER, EMAIL, MANAGERID, DEPARTMENTID, PASSWORD)
                               VALUES(@FIRSTNAME,@OTHERNAME,@LASTNAME,@NATIONALIDENTIFICATIONNUMBER,@MOBILENUMBER,@EMAIL,@MANAGERID,@DEPARTMENTID,@PASSWORD);";
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
                new SqlParameter("@PASSWORD", GetPropertyValue(account.Password))
        };
            
           
            Response<bool> insertAccountResult = await Task.Run(() => _dataBaseUtil.AffectedRowsAsync(insertAccount, parameters));
            Response<bool> insertRoleResult = await Task.Run(() => SetRoleAsync(account.Email, account.RoleId));

            return (insertAccountResult.Success == true && insertRoleResult.Success == true) ?
                new Response<bool>() { Data = { true }, Success = true } :
                new Response<bool>() { Data = { false }, Success = false };
        }
        public async Task<Response<Account>> AuthenticateAsync(string email)
        {
            string query = $@"SELECT PASSWORD FROM ACCOUNT WHERE EMAIL = @EMAIL";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter($"@EMAIL", email) };
            return await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
        }
        public async Task<Response<bool>> DuplicatedAsync(Dictionary<string, object> conditions)
        {
            Response<bool> duplicatedresult = new Response<bool>();
            foreach (var condition in conditions)
            {
                string query = $@"SELECT TOP  1 * FROM {tableName} 
                                  WHERE {condition.Key} = @{condition.Key} ;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                Response<Account> queryResult = await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
                duplicatedresult.Data.Add(queryResult.Data.Count > 0);
                duplicatedresult.Success = queryResult.Success;
            }
            return duplicatedresult;
        }
        public async Task<Response<Account>> GetAsync(Dictionary<string, object> conditions = null)
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
            Response<Account> queryResult =  (conditions == null) ?
                                        await _dataBaseUtil.ExecuteQueryAsync(query) :
                                        await _dataBaseUtil.ExecuteQueryAsync(query, parameters);
            return new Response<Account>() { Success = true, Data = { queryResult.Data.FirstOrDefault() } };
        }
        public async Task<Response<Account>> GetAllAsync(Dictionary<string, object> conditions = null)
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
        public async Task<Response<Account>> GetActiveRequestEmployeeAsync(int managerId)
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
        public async Task<Response<Account>> GetManagerListAsync()
        {
            
            string query = $@"SELECT * FROM ACCOUNT INNER JOIN ACCOUNTROLE 
                              ON ACCOUNT.ACCOUNTID = ACCOUNTROLE.ACCOUNTID WHERE ACCOUNTROLE.ROLEID = @ROLEID1 OR ACCOUNTROLE.ROLEID = @ROLEID2";
            List<SqlParameter> parameters = new List<SqlParameter>() { 
                new SqlParameter($"@ROLEID1", RoleEnum.Manager), 
                new SqlParameter($"@ROLEID2", RoleEnum.Administrator)
            };
            return await _dataBaseUtil.ExecuteQueryAsync(query,parameters);
        }
        public async Task<Response<bool>> SetRoleAsync(string email, int roleId)
        {
            string query = @"INSERT INTO ACCOUNTROLE(ACCOUNTID, ROLEID) VALUES((SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL), @ROLEID);";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@EMAIL", email),
                new SqlParameter("@ROLEID", roleId)
            };
            return await Task.Run(() => _dataBaseUtil.AffectedRowsAsync(query, parameters));
        }
    }
}
