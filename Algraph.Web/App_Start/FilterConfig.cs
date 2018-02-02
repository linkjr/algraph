using System.Web;
using System.Web.Mvc;
using Algraph.Infrastructure.Web.Mvc;

namespace Algraph.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomErrorFilterAttribute());
        }
    }
}
