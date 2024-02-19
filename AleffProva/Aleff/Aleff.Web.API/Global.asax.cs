using Aleff.CrossCutting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.WebApi;

namespace Aleff.Web.API
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      UnityConfig.RegisterComponents();
      //DependencyResolver.SetResolver(new UnityDependencyResolver(DependencyInjector.Container()));
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
  }
}
