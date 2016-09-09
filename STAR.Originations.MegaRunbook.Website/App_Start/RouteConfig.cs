using System.Web.Mvc;
using System.Web.Routing;

namespace STAR.Originations.MegaRunbook.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            const string name = "Default";
            const string url = "{controller}/{action}/{id}";

            routes.MapRoute(name, url, new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
