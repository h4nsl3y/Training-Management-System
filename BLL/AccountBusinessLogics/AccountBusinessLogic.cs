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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.AccountBusinessLogics
{
    public class AccountBusinessLogic : IAccountBusinessLogic
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;
        private readonly Response<Account> _resultError;
        private readonly Response<bool> _resultBoolError;
        public AccountBusinessLogic( IAccountRepository accountRepository, ILogger logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _resultError = new Response<Account> { Success = false, Message = "an Error has been encounter" };
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<Account>> AddAccountAsync(Account account)
        {
            try
            {
                account.Password = await Task.Run(() => Encrypt(account.Password));
                Response<Account> response = await _accountRepository.RegisterAccountAsync(account);
                response.Data.First().Password = "";
                return new Response<Account>
                {
                    Success = true,
                    Data = { response.Data.First() }
                };
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
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
                if (result.Success && result.Data.Any() && result.Data.First()!= null)
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
        public async Task<Response<bool>> IsDuplicatedAsync(string email, string NationalIdentificationNumber, string mobileNumber)
        {
            try
            {
                Dictionary<string, object> conditions = new Dictionary<string, object>()
                {
                    {"EMAIL", email },
                    {"MOBILENUMBER", mobileNumber},
                    {"NATIONALIDENTIFICATIONNUMBER", NationalIdentificationNumber}
                };


                Response<bool> duplicatedResult = await _accountRepository.DuplicatedAsync(conditions);
                if (duplicatedResult.Data.Any(item => item == true))
                {
                    string duplicateMessage = "The field(s) : ";
                    if (duplicatedResult.Data[0] == true) { duplicateMessage += "'email',"; }
                    if (duplicatedResult.Data[1] == true) { duplicateMessage += "'mobile number',"; }
                    if (duplicatedResult.Data[2] == true) { duplicateMessage += "'national identification number',"; }
                    duplicateMessage = duplicateMessage.Remove(duplicateMessage.Length - 1, 1);

                    int lastCommaIndex = duplicateMessage.LastIndexOf(',');
                    if (lastCommaIndex >= 0)
                    {
                        duplicateMessage = duplicateMessage.Substring(0, lastCommaIndex) + " and " + duplicateMessage.Substring(lastCommaIndex + 1);
                    }
                    

                    duplicateMessage += " have already been registered ! ";
                    return new Response<bool> { Success = true, Data = { true }, Message = duplicateMessage };
                }
                else
                {
                    return new Response<bool> { Success = false, Data = { false } };
                }
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public string Encrypt(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
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
                Response<Account> response =  await _accountRepository.GetAsync(conditions);
                if (response.Data.Any())
                {
                    response.Data.First().Password = "";
                }
                return response;
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