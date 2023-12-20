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
            if (controller != null && controller.Session["Role"] != null)
            {
                RoleEnum sessionAccountRole = (RoleEnum)controller.Session["Role"];
                RoleEnum currentRole = sessionAccountRole;
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
