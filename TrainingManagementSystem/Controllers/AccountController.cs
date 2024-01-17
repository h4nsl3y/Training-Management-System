using BLL.AccountBusinessLogics;
using BLL.GenericBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace TrainingManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericBusinessLogic<Account> _genericBusinessLogic;
        private readonly IAccountBusinessLogic _accountBusinessLogic;
        private readonly string primaryKey;
        public AccountController(IGenericBusinessLogic<Account> genericBusinessLogic, IAccountBusinessLogic accountBusinesslogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _accountBusinessLogic = accountBusinesslogic;
            PropertyInfo[] properties = typeof(Account).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        public ActionResult LogInPage() => View();
        public ActionResult RegisterPage() => View();
        public ActionResult RoleSelectionPage() 
        {
            ViewBag.Role = (int)Session["TrueRole"];
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AuthenticateUser(Account account)
        {
            Response<bool> boolResult = await _accountBusinessLogic.AuthenticatedAsync(account.Email, account.Password);
            if (boolResult.Success)
            {
                if (boolResult.Data.Any() && boolResult.Data[0])
                {
                    Response<Account> _accountResult = await _accountBusinessLogic.GetByEmailAsync(account.Email);
                    if (_accountResult.Success)
                    {
                        Account user = _accountResult.Data.First();
                        Session["Account"] = user;
                        Session["TrueRole"] = user.RoleId;
                        return Json(new { Success = true });
                    }
                    else { return Json(new { Success = false, _accountResult.Message }); }
                }
                else{ return Json(new { Success = false, Message = "Wrong email or password" });};
            }
            else { return Json(new { Success = false, boolResult.Message }); }
        }
        [HttpGet]
        public async Task<JsonResult> GetAccount(int accountId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>
            {
                { primaryKey, accountId }
            };
            Response<Account> accountResult = await _accountBusinessLogic.GetAccountAsync(conditions);
            return Json(accountResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllAccount() => Json(await _accountBusinessLogic.GetAllAccountAsync(), JsonRequestBehavior.AllowGet);
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LogUserOut()
        {
            Session.Abandon();
            return Json(new { Success = true });
        }
        [HttpPost]
        public async Task<JsonResult> RegisterUser(Account newAccount)
        {
            Response<bool> duplicatedResult = await _accountBusinessLogic.IsDuplicatedAsync(newAccount.Email, newAccount.NationalIdentificationNumber, newAccount.MobileNumber);
            if (duplicatedResult.Data.Any(item => item == true))
            {
                return Json(new { Success = false, Message = duplicatedResult.Message });
            }
            else
            {
                Response<Account> addResult = await _accountBusinessLogic.AddAccountAsync(newAccount);
                if (addResult.Success)
                {
                    Account user  = addResult.Data.First();
                    Session["Account"] = user;
                    Session["TrueRole"] = user.RoleId;
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false , Message = addResult.Message });
                }
            }
        }
        public ActionResult SetRole(int roleId)
        {
            Session["Role"] = roleId;
            return Json(new { Success = true });
        }
        [HttpGet]
        public async Task<JsonResult> GetEmployeeEnrolled()
        =>  Json(await _accountBusinessLogic.GetActiveRequestEmployeeAsync((int)((Account)Session["Account"]).AccountId), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public async Task<JsonResult> GetManagerList()
        {
            Response<Account> result = await _accountBusinessLogic.GetManagerListAsync();
            var managerNames = result.Data.Select(manager => new {Fullname = $"{manager.FirstName} {manager.OtherName} {manager.LastName}",Value = manager.AccountId}).ToList();
            return  (result.Success) ?
                Json(new { result.Success, Data = managerNames }, JsonRequestBehavior.AllowGet):
                Json(new { result.Success, result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}

