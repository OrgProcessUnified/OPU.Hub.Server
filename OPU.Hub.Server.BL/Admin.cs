using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ErrorEx = OPU.Common.ErrorAndException;
using CHelper = OPU.Common.Helper;
using Model = OPU.Common.Model;
using OPU.Hub.Server.DAL;
using OPU.Server.Helper;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using OPU.Common.Model.Composite;

namespace OPU.Hub.Server.BL
{
    public class Admin
    {
        private DAL.Admin __dal;
        private DAL.Admin _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.Admin());
            }
        }

        public Dictionary<string, object> GetSyncAdminData(string sessionUserName)
        {
            {
                try
                {
                    var ds = _dal.GetSyncAdminData(sessionUserName);

                    var result = new Dictionary<string, object>();

                    result.Add("AdminModules", Model.AdminModule.FromDataTable(ds.Tables[0]));
                    result.Add("AdminModuleActions", Model.AdminModuleAction.FromDataTable(ds.Tables[1]));
                    result.Add("AdminRoles", Model.AdminRole.FromDataTable(ds.Tables[2]));
                    result.Add("AdminRoleModules", Model.AdminRoleModule.FromDataTable(ds.Tables[3]));
                    result.Add("AdminRoleModuleActions", Model.AdminRoleModuleAction.FromDataTable(ds.Tables[4]));
                    result.Add("AdminUsers", Model.AdminUser.FromDataTable(ds.Tables[5]));
                    result.Add("AdminUserRoles", Model.AdminUserRole.FromDataTable(ds.Tables[6]));

                    return result;
                }
                catch (ErrorEx.OPUException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    CHelper.LogHelper.Error(ex);
                    throw new ErrorEx.OPUException(ex);
                }
            }

        }
    }
}
