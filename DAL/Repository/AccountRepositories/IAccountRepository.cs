using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.AccountRepositories
{
    public interface IAccountRepository
    {
        Task<Result<bool>> AddAsync(Account account);
        Task<Result<Account>> AuthenticateAsync(string email);
        Task<Result<bool>> DuplicatedAsync(Dictionary<string, object> conditions);
        Task<Result<Account>> GetAsync(Dictionary<string, object> conditions = null);
        Task<Result<Account>> GetAllAsync(Dictionary<string, object> conditions = null);
        Task<Result<Account>> GetActiveRequestEmployeeAsync(int managerId);
        Task<Result<Account>> GetManagerListAsync();
        Task<Result<bool>> SetRoleAsync(string email, int roleId);
       /* Task<Result<Account>> GetLastRegisteredAccountAsync();*/
    }
}
