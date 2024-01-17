using DAL.Entity;
using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace DAL.Custom
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public RoleEnum AuthorizedRole { get; set; }
        public CustomAuthorizationAttribute(RoleEnum roles)
        {
            AuthorizedRole = roles;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            Account userAccount = (Account)controller.Session["Account"];
            if (controller != null && userAccount.RoleId != null)
            {
                RoleEnum currentRole = (RoleEnum)userAccount.RoleId;
                if (AuthorizedRole != currentRole)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Error", action = "PagenotFound" }
                        )
                    );
                }
            }
        }
    }
}
