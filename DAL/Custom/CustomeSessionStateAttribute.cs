using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace DAL.Custom
{
    public class CustomSessionStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            Account userAccount = (Account)controller.Session["Account"];
            if (controller != null && userAccount.AccountId == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Account", action = "LogInPage" }
                    )
                );
            }
        }
    }
}
