using DAL.Entity;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.GenericRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.AccountBusinessLogics
{
    public class AccountBusinessLogic : IAccountBusinesslogic
    {
        private readonly IGenericRepository<Account> _genericRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;
        private Result<Account> _resultError;
        private Result<bool> _resultBoolError;
        public AccountBusinessLogic(IGenericRepository<Account> genericRepository,IAccountRepository accountRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _accountRepository = accountRepository;
            _logger = logger;
            _resultError = new Result<Account> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Result<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public Result<bool> Add(Account account)
        {
            try
            {
                return _accountRepository.Add(account);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public Result<bool> Authenticated(string email, string password)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {
                    {"EMAIL", email }
                };
                Result<Account> result = _accountRepository.Get(conditions);
                if (result.Success)
                {
                    object passwordHash = result.Data.FirstOrDefault().Password;
                    bool authenticateFlag = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash.ToString());

                    Result<bool> booleanResult = new Result<bool> { Success = true, Data = { authenticateFlag } };
                    return booleanResult;
                }
                else { return _resultBoolError; }
                
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public Result<bool> Duplicated(string email, string NationalIdentificationNumber, string mobileNumber)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {
                    {"EMAIL", email },
                    {"MOBILENUMBER", mobileNumber},
                    {"NATIONALIDENTIFICATIONNUMBER", NationalIdentificationNumber}
                };
                return _accountRepository.Duplicated(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public string Encrypt(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }
        public Result<Account> Get(Dictionary<string, object> conditions = null)
        {
            try
            {
                return _accountRepository.Get(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<Account> GetAll(Dictionary<string, object> conditions = null)
        {
            try
            {
                return _accountRepository.GetAll(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }

        }
        public Result<Account> GetActiveRequestEmployee(int managerId)
        {
            try
            {
                return _accountRepository.GetActiveRequestEmployee(managerId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<Account> GetByEmail(string email)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {{"EMAIL", email }};
                return _accountRepository.Get(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<Account> GetManagerList()
        {
            try
            {
                return _accountRepository.GetManagerList();
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public Result<Account> GetLastRegisteredAccount()
        {
            try
            {
                return _accountRepository.GetLastRegisteredAccount();
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
    }
}