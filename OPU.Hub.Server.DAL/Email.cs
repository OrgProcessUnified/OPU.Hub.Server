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
using CompositeModel = OPU.Common.Model.Composite;

namespace OPU.Hub.Server.DAL
{
    public class Email : DALBase
    {
        #region Direct Calls from Sync Services
        public DataSet Sync_CheckEmailFailure(SqlConnection cnn, string sessionUserName, string emailId, string emailType, string message)
        {
            var cmd = new SqlCommand("admin_usp_CheckEmailEmailFailure", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputVarchar(cmd, "@EmailId", Constants.Common.EmailId, emailId);
            _parameterHelper.AddInputVarchar(cmd, "@EmailType", Constants.Common.EmailType, emailType);
            _parameterHelper.AddInputNVarchar(cmd, "@Message", Constants.Common.EmailMessageParams, message);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            /***************************************************************
             *                   VER VERY IMPORTANT                        *
             * *************************************************************
            Do not use DataAdapter as the Connection object must reamin open
            -----------------------------------------------------------------
            */

            var dr = cmd.ExecuteReader();
            try
            {
                _errorHandler.HandleError(cmd);
            }
            catch (Exception ex)
            {
                if (!dr.IsClosed)
                    dr.Close();

                throw ex;
            }

            return CHelper.ADOHelper.DataReaderToDataSet(dr);
        }

        #endregion Direct Calls from Sync Services

        #region WebAPI Calls
        public void SendEmail(string sessionUserName, string emailType, string emailId, string messageParams)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("admin_usp_SendEmail", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputVarchar(cmd, "@EmailType", Constants.Common.EmailType, emailType);
            _parameterHelper.AddInputVarchar(cmd, "@EmailId", Constants.Common.EmailId, emailId);
            _parameterHelper.AddInputVarchar(cmd, "@MessageParams", Constants.Common.EmailMessageParams, messageParams);

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
