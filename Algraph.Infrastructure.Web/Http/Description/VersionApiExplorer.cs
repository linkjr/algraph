using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace Algraph.Infrastructure.Web.Http.Description
{
    public class VersionApiExplorer : ApiExplorer, IApiExplorer
    {
        private Collection<ApiDescription> apiDesc = null;

        public VersionApiExplorer(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public new Collection<ApiDescription> ApiDescriptions
        {
            get
            {
                if (apiDesc != null)
                    return this.apiDesc;

                apiDesc = new Collection<ApiDescription>();
                foreach (var item in base.ApiDescriptions)
                {
                    var path = item.RelativePath;
                    //When use config.MapHttpAttributeRoutes()[RoutePrefix(),Route() ex.],the type of item.Route is System.Web.Http.Routing.HttpRoute;
                    //else if use config.Routes.MapHttpRoute(),it's System.Web.Http.WebHost.Routing.HostedHttpRoute.
                    //but when it's HostedHttpRoute, it will map all template route, so let's divide it.
                    if (!(item.Route is HttpRoute))
                    {
                        var namespaceConstraint = item.Route.Defaults["namespace"];
                        var controllerType = item.ActionDescriptor.ControllerDescriptor.ControllerType;
                        var assemblyName = controllerType.Assembly.GetName().Name;
                        if (assemblyName != namespaceConstraint + string.Empty)
                            continue;

                        item.RelativePath = item.RelativePath.Replace(assemblyName + ".", string.Empty);
                    }
                    apiDesc.Add(item);
                }

                return apiDesc;
            }
        }
    }
}
