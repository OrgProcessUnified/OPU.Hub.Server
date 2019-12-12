using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace OPU.Hub.Server.WebAPI
{
    public class ApiIdentity : IIdentity
    {
        public string User
        {
            get;
            private set;
        }
        public ApiIdentity(string user)
        {
            this.User = user;
        }

        public string Name
        {
            get
            {
                return this.User;
            }
        }

        public string AuthenticationType
        {
            get
            {
                return "Basic";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }
    }
}