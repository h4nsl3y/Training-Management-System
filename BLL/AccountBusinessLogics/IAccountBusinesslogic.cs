using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AccountBusinessLogics
{
     public interface IAccountBusinessLogic
    {
        Task<Response<Account>> AddAccountAsync(Account account);
        Task<Response<bool>> AuthenticatedAsync(string email, string password);
        Task<Response<bool>> IsDuplicatedAsync(string email, string NationalIdentificationNumber, string mobileNumber);
        string Encrypt(string password);
        Task<Response<Account>> GetAccountAsync(Dictionary<string, object> conditions = null);
        Task<Response<Account>> GetAllAccountAsync(Dictionary<string, object> conditions = null);
        Task<Response<Account>> GetActiveRequestEmployeeAsync(int managerId);
        Task<Response<Account>> GetByEmailAsync(string email);
        Task<Response<Account>> GetManagerListAsync();
    }
}
