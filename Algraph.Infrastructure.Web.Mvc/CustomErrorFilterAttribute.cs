using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Algraph.Infrastructure.Web.Mvc
{
    public class CustomErrorFilterAttribute : HandleErrorAttribute// FilterAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (filterContext.IsChildAction)
                return;
            if (filterContext.ExceptionHandled) //|| !filterContext.HttpContext.IsCustomErrorEnabled
                return;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { result = true, msg = "服务器正忙，请稍后再试" }
                };
            }
            else
            {
                var exception = filterContext.Exception;
                var controllerName = filterContext.RouteData.GetRequiredString("controller");
                var actionName = filterContext.RouteData.GetRequiredString("action");
                var model = new HandleErrorInfo(exception, controllerName, actionName);
                filterContext.Result = new ViewResult
                {
                    ViewName = base.View,
                    MasterName = base.Master,
                    ViewData = new ViewDataDictionary(model),
                    TempData = filterContext.Controller.TempData
                };
            }
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            //filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
