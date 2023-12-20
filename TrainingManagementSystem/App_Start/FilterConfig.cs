using DAL.Custom;
using DAL.Logger;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new CustomHandleErrorAttribute(logger));
        }
    }
}
