using System.Web.Mvc;
using System.Web.Routing;

namespace AvalancheGamesWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "Page",
            url: "{controller}/Page/{PageNumber}/{PageSize}",
            defaults: new { controller = "Role", action = "Page", PageNumber = 0, PageSize = ApplicationConfig.DefaultPageSize }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
