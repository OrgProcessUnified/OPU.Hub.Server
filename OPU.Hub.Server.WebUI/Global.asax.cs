using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OPU.Server.Helper;

namespace OPU.Hub.Server.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            #region Set Generic Configuration
            AppGenericConfiguration.OPUConnectionString = Properties.Settings.Default.OPUConnectionString;
            AppGenericConfiguration.OPUConnectionString2 = Properties.Settings.Default.OPUConnectionString2;
            #endregion Set Generic Configuration
        }
    }
}
