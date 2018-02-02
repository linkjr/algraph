using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using Algraph.Api.Areas.HelpPage;
using Algraph.Infrastructure.Net.Http;
using Algraph.Infrastructure.Web.Http.Description;
using Algraph.Infrastructure.Web.Http.Dispatcher;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Algraph.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.MessageHandlers.Add(new BasicAuthenticationHandler());
            //config.Filters.Add(new AuthorizeAttribute());

            //服务。
            config.Services.Replace(typeof(IApiExplorer), new VersionApiExplorer(config));
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
            config.Services.Replace(typeof(IDocumentationProvider), new XmlDocumentationProvider(AppDomain.CurrentDomain.BaseDirectory + "bin/Algraph.Api.XML"));

            // Web API 路由
            //映射特性定义路由。
            config.MapHttpAttributeRoutes();
            //映射模板路由。
            config.Routes.MapHttpRoute(
                name: "V2",
                routeTemplate: "api/v2/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, @namespace = "Algraph.Rest.V2" },
                constraints: new { @namespace = "Algraph.Rest.V2" }
            );
            config.Routes.MapHttpRoute(
                name: "V1",
                routeTemplate: "api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, @namespace = "Algraph.Rest.V1"},
                constraints: new { @namespace = "Algraph.Rest.V1" }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, @namespace = "Algraph.Api" },
                constraints: new { @namespace = "Algraph.Api" }
            );
        }
    }
}
