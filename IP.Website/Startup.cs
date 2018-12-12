using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IP.Website.Startup))]
namespace IP.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
