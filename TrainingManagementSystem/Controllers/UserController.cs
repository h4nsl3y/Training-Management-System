using DAL.Custom;
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
            ViewBag.FirstName = Session["FirstName"];
            ViewBag.OtherName = Session["OtherName"];
            ViewBag.LastName = Session["LastName"];
            return View();
        }
        [CustomSessionState]
        [CustomAuthorization(RoleEnum.Manager)]
        public ActionResult ManagerViewPage()
        {
            ViewBag.FirstName = Session["FirstName"];
            ViewBag.OtherName = Session["OtherName"];
            ViewBag.LastName = Session["LastName"];
            return View();
        }
        [CustomSessionState]
        [CustomAuthorization(RoleEnum.Administrator)]
        public ActionResult AdministratorViewPage()
        {
            ViewBag.FirstName = Session["FirstName"];
            ViewBag.OtherName = Session["OtherName"];
            ViewBag.LastName = Session["LastName"];
            return View();
        }
    }
}