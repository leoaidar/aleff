using Aleff.CrossCutting;
using Aleff.Web.MVC.Controllers;
using Aleff.Web.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Aleff.Web.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Init();
            var container = DependencyInjector.Container();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      //; ; container.RegisterType<typeof(IUserStore<>), typeof(UserStore<>>;
          // Will resolve both concrete types

//			    container.RegisterType<IUserStore<ApplicationUser>,UserStore<ApplicationUser>>();
//          container.RegisterType<AccountController>(new InjectionConstructor());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
