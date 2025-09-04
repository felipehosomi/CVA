using CVA.Portal.Producao.BLL.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CVA.Portal.Producao.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UserFieldsBLL.CreateUserFields(
                ConfigurationManager.AppSettings["Server"],
                ConfigurationManager.AppSettings["Database"],
                Convert.ToInt32(ConfigurationManager.AppSettings["ServerType"]),
                ConfigurationManager.AppSettings["B1User"],
                ConfigurationManager.AppSettings["B1Password"]
            ); 
        }
    }
}
