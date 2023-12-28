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
        private Response<Account> _resultError;
        private Response<bool> _resultBoolError;
        public AccountBusinessLogic(IGenericRepository<Account> genericRepository, IAccountRepository accountRepository, ILogger logger)
        {
            _genericRepository = genericRepository;
            _accountRepository = accountRepository;
            _logger = logger;
            _resultError = new Response<Account> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<bool>> AddAccountAsync(Account account)
        {
            try
            {
                return await _accountRepository.AddAsync(account);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Response<bool>> AuthenticatedAsync(string email, string password)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {
                    {"EMAIL", email }
                };
                Response<Account> result = await _accountRepository.GetAsync(conditions);
                if (result.Success && result.Data.First() != null)
                {
                    object passwordHash = result.Data.FirstOrDefault().Password;
                    bool authenticateFlag = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash.ToString());

                    return new Response<bool> { Success = true, Data = { authenticateFlag } };
                }
                else {
                    return new Response<bool> { Success = false, Message = "No account linked to this email adddress" };
                }
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Response<bool>> DuplicatedAsync(string email, string NationalIdentificationNumber, string mobileNumber)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {
                    {"EMAIL", email },
                    {"MOBILENUMBER", mobileNumber},
                    {"NATIONALIDENTIFICATIONNUMBER", NationalIdentificationNumber}
                };
                return await _accountRepository.DuplicatedAsync(conditions);
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
        public async Task<Response<Account>> GetAccountAsync(Dictionary<string, object> conditions = null)
        {
            try
            {
                return await _accountRepository.GetAsync(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Account>> GetAllAccountAsync(Dictionary<string, object> conditions = null)
        {
            try
            {
                return await _accountRepository.GetAllAsync(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }

        }
        public async Task<Response<Account>> GetActiveRequestEmployeeAsync(int managerId)
        {
            try
            {
                return await _accountRepository.GetActiveRequestEmployeeAsync(managerId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Account>> GetByEmailAsync(string email)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {{"EMAIL", email }};
                return await _accountRepository.GetAsync(conditions);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<Account>> GetManagerListAsync()
        {
            try
            {
                return await _accountRepository.GetManagerListAsync();
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
    }
}