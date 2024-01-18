using DAL.Custom;
using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class UserController : Controller
    {
        [CustomSessionState]
        [CustomAuthorization(RoleEnum.Employee)]
        public ActionResult EmployeeViewPage()
        {
            ViewBag.FirstName = ((Account)Session["Account"])?.FirstName ?? "";
            ViewBag.OtherName = ((Account)Session["Account"])?.OtherName ?? "";
            ViewBag.LastName = ((Account)Session["Account"])?.LastName ?? "";
            return View();
        }
        [CustomSessionState]
        [CustomAuthorization(RoleEnum.Manager)]
        public ActionResult ManagerViewPage()
        {
            ViewBag.FirstName = ((Account)Session["Account"])?.FirstName ?? "";
            ViewBag.OtherName = ((Account)Session["Account"])?.OtherName ?? "";
            ViewBag.LastName = ((Account)Session["Account"])?.LastName ?? "";
            return View();
        }
        [CustomSessionState]
        [CustomAuthorization(RoleEnum.Administrator)]
        public ActionResult AdministratorViewPage()
        {
            ViewBag.FirstName = ((Account)Session["Account"])?.FirstName ?? "";
            ViewBag.OtherName = ((Account)Session["Account"])?.OtherName ?? "";
            ViewBag.LastName = ((Account)Session["Account"])?.LastName ?? "";
            return View();
        }
    }
}