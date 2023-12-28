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
        Task<Response<bool>> AddAsync(Account account);
        Task<Response<Account>> AuthenticateAsync(string email);
        Task<Response<bool>> DuplicatedAsync(Dictionary<string, object> conditions);
        Task<Response<Account>> GetAsync(Dictionary<string, object> conditions = null);
        Task<Response<Account>> GetAllAsync(Dictionary<string, object> conditions = null);
        Task<Response<Account>> GetActiveRequestEmployeeAsync(int managerId);
        Task<Response<Account>> GetManagerListAsync();
        Task<Response<bool>> SetRoleAsync(string email, int roleId);
       /* Task<Response<Account>> GetLastRegisteredAccountAsync();*/
    }
}
