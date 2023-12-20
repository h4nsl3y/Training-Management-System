using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.AccountRepositories
{
    public interface IAccountRepository
    {
        Result<bool> Add(Account account);
        Result<bool> Duplicated(Dictionary<string, object> conditions);
        Result<Account> Get(Dictionary<string, object> conditions = null);
        Result<Account> GetAll(Dictionary<string, object> conditions = null);
        Result<Account> GetActiveRequestEmployee(int managerId);
        Result<Account> GetLastRegisteredAccount();
        Result<Account> GetManagerList();
        Result<bool> SetRole(string email, int roleId);
    }
}
