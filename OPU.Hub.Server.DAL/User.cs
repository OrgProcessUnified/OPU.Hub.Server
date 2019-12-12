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
    public class User : DALBase
    {
        #region WebAPI Calls
        public void SyncUser(string sessionUserName, string oldUserEmailId, string newUserEmailId)
        {

            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("admin_usp_SyncUser", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);
            _parameterHelper.AddInputVarchar(cmd, "@OldUserEmailId", Constants.Common.EmailId, oldUserEmailId);
            _parameterHelper.AddInputVarchar(cmd, "@NewUserEmailId", Constants.Common.EmailId, newUserEmailId);

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

        #endregion WebAPI Calls

    }
}
