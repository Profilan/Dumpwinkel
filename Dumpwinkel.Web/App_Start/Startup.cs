using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dumpwinkel.Web.Startup))]
namespace Dumpwinkel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}