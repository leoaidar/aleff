using System.Web;
using System.Web.Mvc;

namespace Aleff.Web.MVC
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
