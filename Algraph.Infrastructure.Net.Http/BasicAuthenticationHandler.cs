using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Algraph.Infrastructure.Net.Http
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                var scheme = request.Headers.Authorization.Scheme;
                if (scheme.Equals("Basic"))
                {
                    var parameter = request.Headers.Authorization.Parameter;
                    if (parameter != null)
                    {
                        //TODO:身份验证，授权TOKEN 解密
                        //var data = System.Web.Security.FormsAuthentication.Decrypt(parameter).UserData;
                        var data = parameter;
                        var index = data.IndexOf(":");
                        var name = data.Substring(0, index);
                        var identity = new GenericIdentity(name);
                        var principal = new GenericPrincipal(identity, null);

                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                            HttpContext.Current.User = principal;
                    }
                }
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
