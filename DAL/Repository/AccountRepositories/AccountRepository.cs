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
        public Result<bool> Add (Account account)
        {
            string insertAccount = $@"INSERT INTO {tableName}
                                     (FIRSTNAME, OTHERNAME, LASTNAME, NATIONALIDENTIFICATIONNUMBER, MOBILENUMBER, EMAIL, MANAGERID, DEPARTMENTID, PASSWORD)
                               VALUES(@FIRSTNAME,@OTHERNAME,@LASTNAME,@NATIONALIDENTIFICATIONNUMBER,@MOBILENUMBER,@EMAIL,@MANAGERID,@DEPARTMENTID,@PASSWORD);";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FIRSTNAME", isDataNull(account.FirstName)));
            parameters.Add(new SqlParameter("@OTHERNAME", isDataNull(account.OtherName)));
            parameters.Add(new SqlParameter("@LASTNAME", isDataNull(account.LastName)));
            parameters.Add(new SqlParameter("@NATIONALIDENTIFICATIONNUMBER", isDataNull(account.NationalIdentificationNumber)));
            parameters.Add(new SqlParameter("@MOBILENUMBER", isDataNull(account.MobileNumber)));
            parameters.Add(new SqlParameter("@EMAIL", isDataNull(account.Email)));
            parameters.Add(new SqlParameter("@MANAGERID", isDataNull(account.ManagerId)));
            parameters.Add(new SqlParameter("@DEPARTMENTID", isDataNull(account.DepartmentId)));
            parameters.Add(new SqlParameter("@PASSWORD", isDataNull(account.Password)));
           
            Result<bool> insertAccountResult = _dataBaseUtil.AffectedRows(insertAccount, parameters);
            Result<bool> insertRoleResult = SetRole(account.Email, account.RoleId);

            Result<bool> result = new Result<bool>();
            result.Success = true;
            result.Data.Add(true);
            return result;
        }
        public Result<bool> SetRole(string email, int roleId)
        {
            string query = @"INSERT INTO ACCOUNTROLE(ACCOUNTID, ROLEID) VALUES((SELECT ACCOUNTID FROM ACCOUNT WHERE EMAIL = @EMAIL), @ROLEID);";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EMAIL", email));
            parameters.Add(new SqlParameter("@ROLEID", roleId));
            return _dataBaseUtil.AffectedRows(query, parameters);
        }




        public Result<Account> Get(Dictionary<string, object> conditions = null)
        {
            string query = $@"SELECT {tableName}.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID,
                            ACCOUNT.PASSWORD, ACCOUNTROLE.ROLEID
                            FROM {tableName} 
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
            Result<Account> queryResult = (conditions == null) ?
                                        _dataBaseUtil.ExecuteQuery(query) :
                                        _dataBaseUtil.ExecuteQuery(query, parameters);
            Result<Account> result = new Result<Account>();
            result.Data.Add(queryResult.Data.FirstOrDefault());
            result.Success = true;
            return result;
        }
        public Result<Account> GetAll(Dictionary<string, object> conditions = null)
        {
            string query = $@"SELECT {tableName}.ACCOUNTID,
                            ACCOUNT.FIRSTNAME, ACCOUNT.OTHERNAME, ACCOUNT.LASTNAME, 
                            ACCOUNT.NATIONALIDENTIFICATIONNUMBER, ACCOUNT.MOBILENUMBER, 
                            ACCOUNT.EMAIL, ACCOUNT.MANAGERID, ACCOUNT.DEPARTMENTID, ACCOUNTROLE.ROLEID
                            FROM {tableName} 
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
                _dataBaseUtil.ExecuteQuery(query) :
                _dataBaseUtil.ExecuteQuery(query, parameters);
        }
        public Result<bool> Duplicated(Dictionary<string, object> conditions)
        {
            Result<bool> duplicatedresult = new Result<bool>();
            foreach (var condition in conditions)
            {
                string query = $@"SELECT TOP  1 * FROM {tableName} 
                                  WHERE {condition.Key} = @{condition.Key} ;";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter($"@{condition.Key}", condition.Value));
                Result<Account> queryResult = _dataBaseUtil.ExecuteQuery(query, parameters);
                duplicatedresult.Data.Add(  queryResult.Data.Count > 0);
                duplicatedresult.Success = queryResult.Success;
            }
            return duplicatedresult;
        }
        public Result<Account> GetActiveRequestEmployee(int managerId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT 
                            {tableName}.ACCOUNTID,
                            {tableName}.FIRSTNAME, {tableName}.OTHERNAME, {tableName}.LASTNAME, 
                            {tableName}.NATIONALIDENTIFICATIONNUMBER, {tableName}.MOBILENUMBER, 
                            {tableName}.EMAIL, {tableName}.MANAGERID, {tableName}.DEPARTMENTID, ACCOUNTROLE.ROLEID
                            FROM {tableName} 
                            INNER JOIN ACCOUNTROLE
                            ON ACCOUNTROLE.ACCOUNTID = {tableName}.{primaryKey} 
                            WHERE MANAGERID = @MANAGERID AND {tableName}.{primaryKey} 
                            IN (SELECT DISTINCT ACCOUNTID FROM ENROLLMENT WHERE STATEID = @STATEID )";
            parameters.Add(new SqlParameter($"@MANAGERID", managerId));
            parameters.Add(new SqlParameter($"@STATEID", EnrollmentStateEnum.Waiting_For_Approval ));
            Result<Account> result = _dataBaseUtil.ExecuteQuery(query,parameters);
            return result;
        }
        public Result<Account> GetLastRegisteredAccount()
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
            Result<Account> result = _dataBaseUtil.ExecuteQuery(query);
            result.Data = new List<Account>() { result.Data.FirstOrDefault() }; 
            return result;
        }
        public Result<Account> GetManagerList()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT *FROM ACCOUNT INNER JOIN ACCOUNTROLE 
                              ON ACCOUNT.ACCOUNTID = ACCOUNTROLE.ACCOUNTID WHERE ACCOUNTROLE.ROLEID = @ROLEID";
            parameters.Add(new SqlParameter($"@ROLEID", RoleEnum.Manager));
            Result<Account> result = _dataBaseUtil.ExecuteQuery(query,parameters);
            return result;
        }
        public Result<Account> Authenticate (string email)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = $@"SELECT PASSWORD FROM ACCOUNT WHERE EMAIL = @EMAIL";
            parameters.Add(new SqlParameter($"@EMAIL", email));
            Result<Account> result = _dataBaseUtil.ExecuteQuery(query, parameters);
            return result;
        }
        public object isDataNull(object value)
        {
            return (value == null) ? DBNull.Value : value;
        }
    }
}
