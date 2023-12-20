using DAL.Custom;
using DAL.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL.Custom
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILogger _logger;
        public CustomHandleErrorAttribute(ILogger logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var errorGuid = Guid.NewGuid();
            filterContext.Controller.ViewData["ErrorGuid"] = errorGuid;

            _logger.Log(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                TempData = filterContext.Controller.TempData
            };
        }
    }
}