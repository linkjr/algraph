using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Algraph.Infrastructure.Net.Http;

namespace Algraph.Rest
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.MessageHandlers.Add(new BasicAuthenticationHandler());

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "rest/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}