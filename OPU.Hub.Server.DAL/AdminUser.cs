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
    public class AdminUser : DALBase
    {
        public DataSet GetUserForAuthentication(Model.AdminUser user)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetAdminUserForAuthentication", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddInputVarchar(cmd, "EmailId", Model.AdminUser.FieldLength.EmailId, user.EmailId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);


            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet GetUsers(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetAdminUsers", cnn);
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

        public DataSet GetUserToEdit(string sessionUserName, int userId, bool getPreRequisites)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetAdminUserToEdit", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@UserId", userId);
            _parameterHelper.AddInputBoolean(cmd, "@GetPreRequisites", getPreRequisites);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;

        }

        public DataSet GetUserPreRequisites(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetAdminUserPreRequisites", cnn);
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

        public int AddUser(string sessionUserName, Model.AdminUser user)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_AddAdminUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddOutputInt(cmd, "@UserId");
            _parameterHelper.AddInputVarchar(cmd, "@EmailId", Model.AdminUser.FieldLength.EmailId, user.EmailId);
            _parameterHelper.AddInputVarchar(cmd, "@FullName", Model.AdminUser.FieldLength.FullName, user.FullName);
            _parameterHelper.AddInputVarchar(cmd, "@PrimaryTelephone", Model.AdminUser.FieldLength.PrimaryTelephone, user.PrimaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@SecondaryTelephone", Model.AdminUser.FieldLength.SecondaryTelephone, user.SecondaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@Address", Model.AdminUser.FieldLength.Address, user.Address);
            _parameterHelper.AddInputVarchar(cmd, "@Street", Model.AdminUser.FieldLength.Street, user.Street);
            _parameterHelper.AddInputVarchar(cmd, "@City", Model.AdminUser.FieldLength.City, user.City);
            _parameterHelper.AddInputVarchar(cmd, "@District", Model.AdminUser.FieldLength.District, user.District);
            _parameterHelper.AddInputVarchar(cmd, "@StateCode", Model.AdminUser.FieldLength.StateCode, user.StateCode);
            _parameterHelper.AddInputVarchar(cmd, "@CountryCode", Model.AdminUser.FieldLength.CountryCode, user.CountryCode);
            _parameterHelper.AddInputVarchar(cmd, "@Zip", Model.AdminUser.FieldLength.Zip, user.Zip);

            var prm = cmd.Parameters.AddWithValue("@Roles", Helper.UDTT.IdHelper.ToSqlDataRecords(user.Roles.Select(r => r.RoleId).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_Id";

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);
                return CHelper.ParsingHelper.SafeInteger(cmd.Parameters["@UserId"].Value);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void UpateUser(string sessionUserName, Model.AdminUser user)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_UpdateAdminUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@UserId", user.UserId);
            _parameterHelper.AddInputVarchar(cmd, "@EmailId", Model.AdminUser.FieldLength.EmailId, user.EmailId);
            _parameterHelper.AddInputVarchar(cmd, "@FullName", Model.AdminUser.FieldLength.FullName, user.FullName);
            _parameterHelper.AddInputVarchar(cmd, "@PrimaryTelephone", Model.AdminUser.FieldLength.PrimaryTelephone, user.PrimaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@SecondaryTelephone", Model.AdminUser.FieldLength.SecondaryTelephone, user.SecondaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@Address", Model.AdminUser.FieldLength.Address, user.Address);
            _parameterHelper.AddInputVarchar(cmd, "@Street", Model.AdminUser.FieldLength.Street, user.Street);
            _parameterHelper.AddInputVarchar(cmd, "@City", Model.AdminUser.FieldLength.City, user.City);
            _parameterHelper.AddInputVarchar(cmd, "@District", Model.AdminUser.FieldLength.District, user.District);
            _parameterHelper.AddInputVarchar(cmd, "@StateCode", Model.AdminUser.FieldLength.StateCode, user.StateCode);
            _parameterHelper.AddInputVarchar(cmd, "@CountryCode", Model.AdminUser.FieldLength.CountryCode, user.CountryCode);
            _parameterHelper.AddInputVarchar(cmd, "@Zip", Model.AdminUser.FieldLength.Zip, user.Zip);
            _parameterHelper.AddInputInt(cmd, "@Version", user.Version);

            var prm = cmd.Parameters.AddWithValue("@Roles", Helper.UDTT.IdHelper.ToSqlDataRecords(user.Roles.Select(r => r.RoleId).ToList()));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_Id";

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

        public void DeleteUser(string sessionUserName, int userId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_DeleteAdminUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@UserId", userId);

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

        public void ActivateUser(string sessionUserName, int userId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_ActivateAdminUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@UserId", userId);

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

        public void DeactivateUser(string sessionUserName, int userId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_DeactivateAdminUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@UserId", userId);

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
