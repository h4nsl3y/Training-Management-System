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
        Result<bool> Add(Account account);
        Result<bool> Authenticated(string email, string password);
        Result<bool> Duplicated(string email, string NationalIdentificationNumber, string mobileNumber);
        string Encrypt(string password);
        Result<Account> Get(Dictionary<string, object> conditions = null);
        Result<Account> GetAll(Dictionary<string, object> conditions = null);
        Result<Account> GetActiveRequestEmployee(int managerId);
        Result<Account> GetByEmail(string email);
        Result<Account> GetManagerList();
        Result<Account> GetLastRegisteredAccount();
    }
}
