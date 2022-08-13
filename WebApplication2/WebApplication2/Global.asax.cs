using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication2
{
    public class MvcApplication:System.Web.HttpApplication
    {
        public static DBConnection DBconn;
        public static string image_path;
        protected void Application_Start()
        {
            
            DBconn = new DBConnection(Server.MapPath("App_Data/DB.accdb"));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
      
    }
}
