using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AccountBusinessLogics
{
     public interface IAccountBusinesslogic
    {
        Task<Result<bool>> AddAccountAsync(Account account);
        Task<Result<bool>> AuthenticatedAsync(string email, string password);
        Task<Result<bool>> DuplicatedAsync(string email, string NationalIdentificationNumber, string mobileNumber);
        string Encrypt(string password);
        Task<Result<Account>> GetAccountAsync(Dictionary<string, object> conditions = null);
        Task<Result<Account>> GetAllAccountAsync(Dictionary<string, object> conditions = null);
        Task<Result<Account>> GetActiveRequestEmployeeAsync(int managerId);
        Task<Result<Account>> GetByEmailAsync(string email);
        Task<Result<Account>> GetManagerListAsync();
    }
}
