using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Articles",
               url: "Articles/",
               defaults: new { controller = "articles",action = "Index",id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "ArticleWithoutID",
                url: "Article/id={id}",
                defaults: new { controller = "Article",action = "Index",id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ArticleEditArticle",
                url: "Article/edit/edit_article",
                defaults: new { controller = "Article",action = "edit_article",id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ArticleEditUI",
                url: "Article/edit/",
                defaults: new { controller = "Article",action = "edit",id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ArticleWithID",
                url: "Article/{action}/{id}",
                defaults: new { controller = "Article",action = "index",id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Article",
                url: "Article/{action}",
                defaults: new { controller = "Article",action = "index",id = UrlParameter.Optional }
            );

            /*routes.MapRoute(
                name: "ArticleWithAction",
                url: "Article/id={id}/{action}",
                defaults: new { controller = "Article",action = "index",id = UrlParameter.Optional }
            );*/
            routes.MapRoute(
                name: "Admin",
                url: "Admin/",
                defaults: new { controller = "Admin",action = "Index",id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "articles",action = "Index",id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default1",
                url: "{*nodeAliasPath}",
                defaults: new { controller = "home",action = "Index",id = UrlParameter.Optional }
            );

        }
    }
}
