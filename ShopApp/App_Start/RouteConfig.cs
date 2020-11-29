using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "RegisterConfirm",
               url: "{controller}/{action}/{email}",
               defaults: new { controller = "User", action = "ConfirmRegistration", email = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "ChangePasswordConfirm",
               url: "{controller}/{action}/{email}/{psw}",
               defaults: new { controller = "UserPanel", action = "ConfirmPasswordChange", email = UrlParameter.Optional, psw = UrlParameter.Optional }
           );
        }
    }
}
