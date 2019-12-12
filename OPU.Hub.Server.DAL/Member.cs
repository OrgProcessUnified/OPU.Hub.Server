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
    public class Member : DALBase
    {
        #region Direct Calls from Sync Services
        public DataSet Sync_GetMember(SqlConnection cnn, string sessionUserName, int memberId)
        {
            var cmd = new SqlCommand("usp_GetMember", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", memberId);

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

        public DataSet Sync_GetMemberAccount(SqlConnection cnn, string sessionUserName, int memberId)
        {
            var cmd = new SqlCommand("usp_GetMemberAccount", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", memberId);

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

        public DataSet Sync_GetPaymentCompletedSyncInfo(SqlConnection cnn, string sessionUserName, int initiatedPaymentId)
        {
            var cmd = new SqlCommand("usp_GetPaymentCompletedSyncInfo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@InitiatedPaymentId", initiatedPaymentId);

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
        public void OnNodeMemberAddFailure(string sessionUserName, int memberId)
        {

            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_OnNodeMemberAddFailure", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);
            _parameterHelper.AddInputInt(cmd, "@MemberId", memberId);

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

        public DataSet GetMemberPreRequisites(string sessionUserName)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetMemberPreRequisites", cnn);
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

        public int AddMember(string sessionUserName, Model.Member model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_AddMember", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddOutputInt(cmd, "@MemberId");
            _parameterHelper.AddInputInt(cmd, "@LicenseCount", model.LicenseCount);
            
            _parameterHelper.AddInputVarchar(cmd, "@MemberEmailId", Model.Member.FieldLength.MemberEmailId, model.MemberEmailId);
            _parameterHelper.AddInputVarchar(cmd, "@MemberName", Model.Member.FieldLength.MemberName, model.MemberName);
            _parameterHelper.AddInputVarchar(cmd, "@TaxIdNumber", Model.Member.FieldLength.TaxIdNumber, model.TaxIdNumber);
            _parameterHelper.AddInputVarchar(cmd, "@TaxIdTypeId", Model.Member.FieldLength.TaxIdTypeId, model.TaxIdTypeId);
            _parameterHelper.AddInputVarchar(cmd, "@PrimaryTelephone", Model.Member.FieldLength.PrimaryTelephone, model.PrimaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@SecondaryTelephone", Model.Member.FieldLength.SecondaryTelephone, model.SecondaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@Address", Model.Member.FieldLength.Address, model.Address);
            _parameterHelper.AddInputVarchar(cmd, "@Street", Model.Member.FieldLength.Street, model.Street);
            _parameterHelper.AddInputVarchar(cmd, "@City", Model.Member.FieldLength.City, model.City);
            _parameterHelper.AddInputVarchar(cmd, "@District", Model.Member.FieldLength.District, model.District);
            _parameterHelper.AddInputVarchar(cmd, "@StateCode", Model.Member.FieldLength.StateCode, model.StateCode);
            _parameterHelper.AddInputVarchar(cmd, "@CountryCode", Model.Member.FieldLength.CountryCode, model.CountryCode);
            _parameterHelper.AddInputVarchar(cmd, "@Zip", Model.Member.FieldLength.Zip, model.Zip);
            _parameterHelper.AddInputInt(cmd, "@SalesOwnerId", model.SalesOwnerId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);

                return CHelper.ParsingHelper.SafeInteger(cmd.Parameters["@MemberId"].Value);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void UpdateMember(string sessionUserName, Model.Member model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_UpdateMember", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);
            _parameterHelper.AddInputVarchar(cmd, "@MemberName", Model.Member.FieldLength.MemberName, model.MemberName);
            _parameterHelper.AddInputVarchar(cmd, "@TaxIdNumber", Model.Member.FieldLength.TaxIdNumber, model.TaxIdNumber);
            _parameterHelper.AddInputVarchar(cmd, "@TaxIdTypeId", Model.Member.FieldLength.TaxIdTypeId, model.TaxIdTypeId);
            _parameterHelper.AddInputVarchar(cmd, "@PrimaryTelephone", Model.Member.FieldLength.PrimaryTelephone, model.PrimaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@SecondaryTelephone", Model.Member.FieldLength.SecondaryTelephone, model.SecondaryTelephone);
            _parameterHelper.AddInputVarchar(cmd, "@Address", Model.Member.FieldLength.Address, model.Address);
            _parameterHelper.AddInputVarchar(cmd, "@Street", Model.Member.FieldLength.Street, model.Street);
            _parameterHelper.AddInputVarchar(cmd, "@City", Model.Member.FieldLength.City, model.City);
            _parameterHelper.AddInputVarchar(cmd, "@District", Model.Member.FieldLength.District, model.District);
            _parameterHelper.AddInputVarchar(cmd, "@StateCode", Model.Member.FieldLength.StateCode, model.StateCode);
            _parameterHelper.AddInputVarchar(cmd, "@CountryCode", Model.Member.FieldLength.CountryCode, model.CountryCode);
            _parameterHelper.AddInputVarchar(cmd, "@Zip", Model.Member.FieldLength.Zip, model.Zip);
            _parameterHelper.AddInputInt(cmd, "@SalesOwnerId", model.SalesOwnerId);
            _parameterHelper.AddInputInt(cmd, "@Version", model.Version);

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

        public string GetMemberHost(string sessionUserName, string memberEmailId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetMemberHost", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputVarchar(cmd, "@MemberEmailId", Model.Member.FieldLength.MemberEmailId, memberEmailId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                var result = cmd.ExecuteScalar();
                _errorHandler.HandleError(cmd);

                return result?.ToString();
            }
            finally
            {
                cnn.Close();
            }
        }

        public DataSet GetMember(string sessionUserName, int memberId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetMember", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", memberId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet GetMemberAccount(string sessionUserName, int memberId)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetMemberAccount", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", memberId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet GetOtherNodeMembers(string sessionUserName, OtherNodeMemberRequest request)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("admin_usp_GetOtherNodeMembers", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputVarchar(cmd, "@RequesterHost", Model.Member.FieldLength.MemberHost, request.RequesterHost);
            _parameterHelper.AddInputDateTime(cmd, "@MemberSyncTime", request.MemberSyncTime);
            _parameterHelper.AddInputDateTime(cmd, "@SupplierSettingSyncTime", request.SupplierSettingSyncTime);
            _parameterHelper.AddInputDateTime(cmd, "@SupplierManufacturerSyncTime", request.SupplierManufacturerSyncTime);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet CalculatePaymentInfo(string sessionUserName, Model.InitiatedPayment initiatedPayment)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_CalculatePaymentInfo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", initiatedPayment.MemberId);
            _parameterHelper.AddInputDecimal(cmd, "@PayForMonths", Model.InitiatedPayment.FieldLength.PayForMonths, Model.InitiatedPayment.FieldPrecision.PayForMonths, initiatedPayment.PayForMonths);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public int AddInitiatedPayment(string sessionUserName, Model.InitiatedPayment model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_AddInitiatedPayment", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddOutputInt(cmd, "@InitiatedPaymentId");
            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);

            _parameterHelper.AddInputDecimal(cmd, "@PayForMonths", Model.InitiatedPayment.FieldLength.PaymentAmount, Model.InitiatedPayment.FieldPrecision.PayForMonths, model.PayForMonths);
            _parameterHelper.AddInputDecimal(cmd, "@RatePerMonth", Model.InitiatedPayment.FieldLength.RatePerMonth, Model.InitiatedPayment.FieldPrecision.RatePerMonth, model.RatePerMonth);
            _parameterHelper.AddInputDecimal(cmd, "@GrossAmount", Model.InitiatedPayment.FieldLength.GrossAmount, Model.InitiatedPayment.FieldPrecision.GrossAmount, model.GrossAmount);
            _parameterHelper.AddInputDecimal(cmd, "@BulkPaymentDiscount", Model.InitiatedPayment.FieldLength.BulkPaymentDiscount, Model.InitiatedPayment.FieldPrecision.BulkPaymentDiscount, model.BulkPaymentDiscount);
            _parameterHelper.AddInputDecimal(cmd, "@BulkPaymentDiscountAmount", Model.InitiatedPayment.FieldLength.BulkPaymentDiscountAmount, Model.InitiatedPayment.FieldPrecision.BulkPaymentDiscountAmount, model.BulkPaymentDiscountAmount);
            _parameterHelper.AddInputDecimal(cmd, "@TaxableAmount", Model.InitiatedPayment.FieldLength.TaxableAmount, Model.InitiatedPayment.FieldPrecision.TaxableAmount, model.TaxableAmount);
            _parameterHelper.AddInputDecimal(cmd, "@GSTRate", Model.InitiatedPayment.FieldLength.GSTRate, Model.InitiatedPayment.FieldPrecision.GSTRate, model.GSTRate);
            _parameterHelper.AddInputDecimal(cmd, "@GSTAmount", Model.InitiatedPayment.FieldLength.GSTAmount, Model.InitiatedPayment.FieldPrecision.GSTAmount, model.GSTAmount);

            _parameterHelper.AddInputInt(cmd, "@LicenseCount", model.LicenseCount);
            _parameterHelper.AddInputBoolean(cmd, "@IsUpgrade", model.IsUpgrade);
            _parameterHelper.AddInputInt(cmd, "@InitiatedPaymentBy", model.InitiatedPaymentBy);
            

            _parameterHelper.AddInputDecimal(cmd, "@PaymentAmount", Model.InitiatedPayment.FieldLength.PaymentAmount, Model.InitiatedPayment.FieldPrecision.PaymentAmount, model.PaymentAmount);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            cnn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                _errorHandler.HandleError(cmd);


                return CHelper.ParsingHelper.SafeInteger(cmd.Parameters["@InitiatedPaymentId"].Value);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void CompleteInitiatedPayment(string sessionUserName, Model.InitiatedPayment model, string PGTransactionReference, bool isDeclined)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_CompleteInitiatedPayment", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@InitiatedPaymentId", model.InitiatedPaymentId);
            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);
            _parameterHelper.AddInputDecimal(cmd, "@ActualPaymentAmount", Model.InitiatedPayment.FieldLength.ActualPaymentAmount, Model.InitiatedPayment.FieldPrecision.ActualPaymentAmount, model.ActualPaymentAmount);
            _parameterHelper.AddInputVarchar(cmd, "@PGTransactionReference", Model.MemberAccountTransaction.FieldLength.PGTransactionReference, PGTransactionReference);
            _parameterHelper.AddInputBoolean(cmd, "@IsDeclined", isDeclined);
            _parameterHelper.AddInputVarchar(cmd, "@PaymentResponseJSON", Model.InitiatedPayment.FieldLength.PaymentResponseJSON, model.PaymentResponseJSON);

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

        public DataSet GetInitiatedPayment(string sessionUserName, Model.InitiatedPayment model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetInitiatedPayment", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@InitiatedPaymentId", model.InitiatedPaymentId);
            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet GetInitiatedPaymentForPaymentGatewayProcessing(string sessionUserName, Model.InitiatedPayment model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_GetInitiatedPaymentForPaymentGatewayProcessing", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@InitiatedPaymentId", model.InitiatedPaymentId);
            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public DataSet CalculateUpgradeInfo(string sessionUserName, Model.UpgradeLicense model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_CalculateUpgradeInfo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);
            _parameterHelper.AddInputInt(cmd, "@UpgradeLicenseCount", model.UpgradeLicenseCount);

            _parameterHelper.AddErrorAndDebugParameters(cmd);

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            var ds = new DataSet();
            da.Fill(ds);
            _errorHandler.HandleError(cmd);
            return ds;
        }

        public void UpgradeLicense(string sessionUserName, Model.UpgradeLicense model)
        {
            var cnn = SQLClientHelper.ConnectionHelper.GetConnection();
            var cmd = new SqlCommand("usp_UpgradeLicenseCount", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            _parameterHelper.AddSessionUserNameParameter(cmd, sessionUserName);

            _parameterHelper.AddInputInt(cmd, "@MemberId", model.MemberId);
            _parameterHelper.AddInputInt(cmd, "@UpgradeLicenseCount", model.UpgradeLicenseCount);
            _parameterHelper.AddInputInt(cmd, "@UpgradeBy", model.UpgradeBy);

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
