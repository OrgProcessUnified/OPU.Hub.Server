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
using OPU.Common.Model.Composite;

namespace OPU.Hub.Server.DAL
{
    public class Admin : DALBase
    {
        #region Direct Calls from Sync Services
        #endregion Direct Calls from Sync Services

        #region WebAPI Calls
        public DataSet GetSyncAdminData(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("admin_usp_GetSyncAdminData", cnn);
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

        #endregion WebAPI Calls

    }
}
