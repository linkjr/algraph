using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Algraph.Web.Startup))]
namespace Algraph.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
