using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Aleff.Web.MVC.Startup))]
namespace Aleff.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
