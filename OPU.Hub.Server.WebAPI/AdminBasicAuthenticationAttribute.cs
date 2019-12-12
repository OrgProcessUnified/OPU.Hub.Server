using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Text;
using System.Security.Principal;

using BL = OPU.Hub.Server.BL;
using Model = OPU.Common.Model;

namespace OPU.Hub.Server.WebAPI
{
    public class AdminBasicAuthenticationAttribute : ActionFilterAttribute
    {
        public AdminBasicAuthenticationAttribute()
        {
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

                var bl = new BL.AdminUser();
                var user = new Model.AdminUser()
                {
                    EmailId = username,
                    Password = password
                };

                var error = bl.ValidateUser(user);
                if (error.Code > 0)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
                else
                {
                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(username), new string[] { });
                    base.OnActionExecuting(actionContext);
                }
            }
        }
    }
}