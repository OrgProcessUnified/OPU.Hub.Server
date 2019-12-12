using OPU.Server.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Fabrik.Common.WebAPI;
namespace OPU.Hub.Server.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new CompressionHandler());

            #region Set Generic Configuration
            AppGenericConfiguration.OPUConnectionString = Properties.Settings.Default.OPUConnectionString;
            AppGenericConfiguration.OPUConnectionString2 = Properties.Settings.Default.OPUConnectionString2;
            #endregion Set Generic Configuration
        }
    }
}
