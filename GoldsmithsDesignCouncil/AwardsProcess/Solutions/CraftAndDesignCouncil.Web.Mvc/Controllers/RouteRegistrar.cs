namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes) 
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}/{id2}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional}); // Parameter defaults
        }
    }
}
