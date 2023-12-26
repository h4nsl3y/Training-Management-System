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
            _logger.Log(filterContext.Exception);

            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.ExceptionHandled = true;
        }
    }
}