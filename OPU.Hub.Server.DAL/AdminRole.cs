using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Model = OPU.Common.Model;
using CHelper = OPU.Common.Helper;
using SQLClientHelper = OPU.Server.Helper.SQLClient;
using ErrorEx = OPU.Common.ErrorAndException;

namespace OPU.Hub.Server.DAL
{
    public class AdminRole : DALBase
    {
        public DataSet GetRoles(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetRoles", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;

        }

        public DataSet GetRolePreRequisites(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetRolePreRequisites", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public int AddRole(string sessionUserName, Model.AdminRole model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_AddRole", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddOutputInt(cmd, "@RoleId");
            _parameterHelper.AddInputVarchar(cmd, "@RoleName", Model.AdminRole.FieldLength.RoleName, model.RoleName);
            _parameterHelper.AddInputVarchar(cmd, "@Description", Model.AdminRole.FieldLength.Description, model.Description);
            _parameterHelper.AddInputBoolean(cmd, "@IsActive", model.IsActive);

            var prm = cmd.Parameters.AddWithValue("@Modules", Helper.UDTT.IdHelper.ToSqlDataRecords(model.Modules.Select(m => m.ModuleId).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_Id";

            prm = cmd.Parameters.AddWithValue("@ModuleActions", Helper.UDTT.MasterDetailHelper.ToSqlDataRecords(model.ModuleActions.Select(a => new Model.MasterDetail() { MasterId = a.ModuleId, DetailId = a.ModuleActionId }).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_MasterDetail";

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);
                return CHelper.ParsingHelper.SafeInteger(cmd.Parameters["@RoleId"].Value);
            }
            finally
            {
                cnn.Close();
            }
        }

        public DataSet GetRoleToEdit(string sessionUserName, int roleId, bool getPreRequisites)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetRoleToEdit", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@RoleId", roleId);
            _parameterHelper.AddInputBoolean(cmd, "@GetPreRequisites", getPreRequisites);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;

        }

        public void UpateRole(string sessionUserName, Model.AdminRole model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_UpdateRole", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@RoleId", model.RoleId);
            _parameterHelper.AddInputVarchar(cmd, "@RoleName", Model.AdminRole.FieldLength.RoleName, model.RoleName);
            _parameterHelper.AddInputVarchar(cmd, "@Description", Model.AdminRole.FieldLength.Description, model.Description);
            _parameterHelper.AddInputBoolean(cmd, "@IsActive", model.IsActive);
            _parameterHelper.AddInputInt(cmd, "@Version", model.Version);

            var prm = cmd.Parameters.AddWithValue("@Modules", Helper.UDTT.IdHelper.ToSqlDataRecords(model.Modules.Select(m => m.ModuleId).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_Id";

            prm = cmd.Parameters.AddWithValue("@ModuleActions", Helper.UDTT.MasterDetailHelper.ToSqlDataRecords(model.ModuleActions.Select(a => new Model.MasterDetail() { MasterId = a.ModuleId, DetailId = a.ModuleActionId }).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_MasterDetail";

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void DeleteRole(string sessionUserName, int roleId, bool removeRoleFromUser = false)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_DeleteRole", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@RoleId", roleId);
            _parameterHelper.AddInputBoolean(cmd, "@RemoveRoleFromUser", removeRoleFromUser);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);
            }
            finally
            {
                cnn.Close();
            }
        }

    }
}
