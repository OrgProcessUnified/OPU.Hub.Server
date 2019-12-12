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
    public class Supplier : DALBase
    {
        #region Direct Calls from Sync Services
        #endregion Direct Calls from Sync Services

        #region WebAPI Calls
        public void UpdateSupplierSetting(string sessionUserName, Model.SupplierSetting model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_UpdateSupplierSetting", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@SupplierId", model.SupplierId);
                        
            _parameterHelper.AddInputBoolean(cmd, "@IsActiveSupplier", model.IsActiveSupplier);
            _parameterHelper.AddInputBoolean(cmd, "@AllowBuyerToReRequestOption", model.AllowBuyerToReRequestOption);
            _parameterHelper.AddInputVarchar(cmd, "@SupplierVisibilityOption", Model.SupplierSetting.FieldLength.SupplierVisibilityOption, model.SupplierVisibilityOption);

            _parameterHelper.AddInputInt(cmd, "@ImportOrderFormatNumber", model.ImportOrderFormatNumber);
            _parameterHelper.AddInputInt(cmd, "@ImportOrderItemRowStartLineNumber", model.ImportOrderItemRowStartLineNumber);
            
            _parameterHelper.AddInputInt(cmd, "@ImportOrderItemManufacturerNameColumnNumber", model.ImportOrderItemManufacturerNameColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ImportOrderItemProductCodeColumnNumber", model.ImportOrderItemProductCodeColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ImportOrderItemProductNameColumnNumber", model.ImportOrderItemProductNameColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ImportOrderItemQuantityColumnNumber", model.ImportOrderItemQuantityColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ExportOrderFormatNumber", model.ExportOrderFormatNumber);
            _parameterHelper.AddInputInt(cmd, "@ExportOrderItemRowStartLineNumber", model.ExportOrderItemRowStartLineNumber);
            _parameterHelper.AddInputVarchar(cmd, "@ExportHeader", Common.Model.SupplierSetting.FieldLength.ExportHeader, model.ExportHeader);

            _parameterHelper.AddInputInt(cmd, "@ExportOrderItemManufacturerNameColumnNumber", model.ExportOrderItemManufacturerNameColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ExportOrderItemProductCodeColumnNumber", model.ExportOrderItemProductCodeColumnNumber);
            _parameterHelper.AddInputInt(cmd, "@ExportOrderItemProductNameColumnNumber", model.ExportOrderItemProductNameColumnNumber);

            _parameterHelper.AddInputInt(cmd, "@ExportOrderItemQuantityColumnNumber", model.ExportOrderItemQuantityColumnNumber);
			_parameterHelper.AddInputBoolean(cmd, "@CalculateOrderValueOnPTR", model.CalculateOrderValueOnPTR);


			_parameterHelper.AddInputInt(cmd, "@Version", model.Version);
            _parameterHelper.AddInputDateTime(cmd, "@UpdatedOn", model.UpdatedOn);

            var prm = cmd.Parameters.AddWithValue("@SupplierSettingStates", UDTT.SupplierSettingStateHelper.ToSqlDataRecords(model.SupplierSettingStates));
            prm.SqlDbType = SqlDbType.Structured;
            prm.TypeName = "dbo.udtt_SupplierSettingState";


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
