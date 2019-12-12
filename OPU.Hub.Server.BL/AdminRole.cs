using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ErrorEx = OPU.Common.ErrorAndException;
using CHelper = OPU.Common.Helper;
using Model = OPU.Common.Model;
using OPU.Hub.Server.DAL;

namespace OPU.Hub.Server.BL
{
    public class AdminRole
    {
        private DAL.AdminRole __dal;
        private DAL.AdminRole _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.AdminRole());
            }
        }

        public List<Model.AdminRole> GetRoles(string sessionUserName)
        {
            try
            {
                var ds = _dal.GetRoles(sessionUserName);

                return Model.AdminRole.FromDataSet(ds);
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
        public Dictionary<string, object> GetRoleToEdit(string sessionUserName, int roleId, bool getPreRequisites)
        {
            try
            {
                var ds = _dal.GetRoleToEdit(sessionUserName, roleId, getPreRequisites);

                var role = Model.AdminRole.FromDataRow(ds.Tables[0].Rows[0]);
                role.Modules = Model.AdminModule.FromDataTable(ds.Tables[1]);
                role.ModuleActions = Model.AdminModuleAction.FromDataTable(ds.Tables[2]);

                var result = new Dictionary<string, object>();
                result.Add("Role", role);

                result.Add("Modules", Model.AdminModule.FromDataTable(ds.Tables[3]));
                result.Add("ModuleActions", Model.AdminModuleAction.FromDataTable(ds.Tables[4]));

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
        public Dictionary<string, object> GetRolePreRequisites(string sessionUserName)
        {
            try
            {
                var ds = _dal.GetRolePreRequisites(sessionUserName);

                var result = new Dictionary<string, object>();
                result.Add("Modules", Model.AdminModule.FromDataTable(ds.Tables[0]));
                result.Add("ModuleActions", Model.AdminModuleAction.FromDataTable(ds.Tables[1]));

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

        public object SaveRole(string sessionUserName, Model.AdminRole role, bool readBack)
        {
            var saveSuccess = false;
            try
            {
                int roleId = role.RoleId;
                if (role.RoleId <= 0) /*Add Role*/
                {
                    roleId = _dal.AddRole(sessionUserName, role);
                }
                else /*Update Role*/
                {
                    _dal.UpateRole(sessionUserName, role);
                }

                saveSuccess = true;
                if (readBack)
                {
                    return GetRoleToEdit(sessionUserName, roleId, true);
                }

                return roleId;
            }
            catch (ErrorEx.OPUException ex)
            {
                if (saveSuccess)
                {
                    return new ErrorEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message,
                        ExceptionMessage = ex.ExceptionMessage
                    };
                }
                throw ex;
            }
            catch (Exception ex)
            {
                CHelper.LogHelper.Error(ex);

                if (saveSuccess)
                {
                    return ErrorEx.Error.GetUnknownError(ex);
                }

                throw new ErrorEx.OPUException(ex);
            }

        }

        public void DeleteRole(string sessionUserName, int roleId)
        {
            try
            {
                _dal.DeleteRole(sessionUserName, roleId);
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
