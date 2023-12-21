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
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericBusinessLogic<Account> _genericBusinessLogic;
        private readonly IAccountBusinesslogic _accountBusinessLogic;
        private readonly string primaryKey;
        public AccountController(IGenericBusinessLogic<Account> genericBusinessLogic, IAccountBusinesslogic accountBusinesslogic)
        {
            _genericBusinessLogic = genericBusinessLogic;
            _accountBusinessLogic = accountBusinesslogic;
            PropertyInfo[] properties = typeof(Account).GetProperties();
            primaryKey = properties.Where(p => Attribute.IsDefined(p, typeof(KeyAttribute))).FirstOrDefault().Name;
        }
        public ActionResult LogInPage()
        {
            return View();
        }
        public ActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AuthenticateUser(Account account)
        {
            Result<bool> boolResult = _accountBusinessLogic.Authenticated(account.Email, account.Password);
            if (boolResult.Success)
            {
                if (boolResult.Data[0])
                {
                    Result<Account> _accountResult = _accountBusinessLogic.GetByEmail(account.Email);
                    if (_accountResult.Success)
                    {
                        Account user = _accountResult.Data.FirstOrDefault();
                        Session["AccountId"] = user.AccountId;
                        Session["FirstName"] = user.FirstName;
                        Session["LastName"] = user.LastName;
                        Session["OtherName"] = user.OtherName;
                        Session["Email"] = user.Email;
                        Session["MobileNumber"] = user.MobileNumber;
                        Session["Role"] = user.RoleId;
                        return Json(new { message = "Success" });
                    }
                    else { return Json(new { message = "Failure", data = _accountResult.Message }); }
                }
                else{ return Json(new { message = "Failure", data = "Wrong email or password" });};
            }
            else { return Json(new { message = "Failure", data = boolResult.Message }); }
        }
        [HttpGet]
        public JsonResult GetAccount(int accountId)
        {
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add(primaryKey, accountId);
            Result<Account> accountResult = _accountBusinessLogic.Get(conditions);
            return  (accountResult.Success) ?
                Json(new { message = "success", data = accountResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllAccount()
        {
            Result<Account> accountResult = _accountBusinessLogic.GetAll();
            return (accountResult.Success) ?
                Json(new { message = "success", data = accountResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LogUserOut()
        {
            Session.Abandon();
            return Json(new { message = "Success" });
        }
        [HttpPost]
        public JsonResult RegisterUser(Account newAccount)
        {
            Result<bool> duplicatedResult = _accountBusinessLogic.Duplicated(newAccount.Email, newAccount.NationalIdentificationNumber, newAccount.MobileNumber);
            if (duplicatedResult.Data.Any(item => item == true))
            {
                string duplicateMessage = "The field(s) : ";
                if (duplicatedResult.Data[0] == true) { duplicateMessage += "email,"; }
                if (duplicatedResult.Data[1] == true) { duplicateMessage += "mobile number,"; }
                if (duplicatedResult.Data[2] == true) { duplicateMessage += "national identification number,"; }
                duplicateMessage = duplicateMessage.Substring(0, duplicateMessage.Length - 1);
                duplicateMessage += " have already been registered ! ";
                return Json(new { message = "Failure", data = duplicateMessage });
            }
            else
            {
                newAccount.Password = _accountBusinessLogic.Encrypt(newAccount.Password);
                Result<bool> addResult = _accountBusinessLogic.Add(newAccount);
                if (addResult.Success)
                {
                    Result<Account> accountResult = _accountBusinessLogic.GetLastRegisteredAccount();
                    if (accountResult.Success)
                    {
                        Account user = accountResult.Data.FirstOrDefault();
                        Session["AccountId"] = user.AccountId;
                        Session["FirstName"] = user.FirstName;
                        Session["LastName"] = user.LastName;
                        Session["OtherName"] = user.OtherName;
                        Session["Email"] = user.Email;
                        Session["MobileNumber"] = user.MobileNumber;
                        Session["Role"] = user.RoleId;
                        return Json(new { message = "Success" });
                    }
                    else { return Json(new { message = "Failure", data = accountResult.Message }); };
                }
                else
                {
                    return Json(new { message = addResult.Message });
                }
            }
        }
        public RedirectToRouteResult RedirectToView()
        {
            RoleEnum actualRole = (RoleEnum)Session["Role"];
            switch (actualRole)
            {
                case RoleEnum.Employee:
                    return RedirectToRoute(new { controller = "Home", action = "EmployeeViewPage" });
                case RoleEnum.Manager:
                    return RedirectToRoute(new { controller = "Home", action = "ManagerViewPage" });
                case RoleEnum.Administrator:
                    return RedirectToRoute(new { controller = "Home", action = "AdministratorViewPage" });
                default:
                    return RedirectToRoute(new { controller = "Error", action = "NotFound" });

            }

        }
        [HttpGet]
        public JsonResult GetEmployeeEnrolled()
        {
            int managerId = (int)Session["AccountId"];
            Result<Account> accountResult = _accountBusinessLogic.GetActiveRequestEmployee(managerId);
            return (accountResult.Success) ?
                Json(new { message = "success", data = accountResult.Data }, JsonRequestBehavior.AllowGet) :
                Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetManagerList()
        {
            Result<Account> accountResult = _accountBusinessLogic.GetManagerList();
            if (accountResult.Success)
            {
                var managerNames = accountResult.Data.Select(manager => new
                {
                    Fullname = $"{manager.FirstName} {manager.OtherName} {manager.LastName}",
                    Value = manager.AccountId
                }).ToList();
                return Json(new { message = "success", data = managerNames }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Failed", data = accountResult.Data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

