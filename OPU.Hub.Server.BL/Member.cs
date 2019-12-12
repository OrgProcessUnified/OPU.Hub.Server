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
    public class Member
    {
        private DAL.Member __dal;
        private DAL.Member _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.Member());
            }
        }

        public Model.Member Sync_GetMember(SqlConnection cnn, string sessionUserName, int memberId)
        {
            try
            {
                DataSet ds = _dal.Sync_GetMember(cnn, sessionUserName, memberId);

                return Model.Member.FromDataRow(ds.Tables[0].Rows[0]);
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

        public Model.MemberAccount Sync_GetMemberAccount(SqlConnection cnn, string sessionUserName, int memberId)
        {
            try
            {
                DataSet ds = _dal.Sync_GetMemberAccount(cnn, sessionUserName, memberId);

                return Model.MemberAccount.FromDataRow(ds.Tables[0].Rows[0]);
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

        public Model.MemberAccountTransaction Sync_GetPaymentCompletedSyncInfo(SqlConnection cnn, string sessionUserName, int initiatedPaymentId)
        {
            try
            {
                DataSet ds = _dal.Sync_GetPaymentCompletedSyncInfo(cnn, sessionUserName, initiatedPaymentId);

                return Model.MemberAccountTransaction.FromDataRow(ds.Tables[0].Rows[0]);
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

        public Model.Member GetMember(string sessionUserName, int memberId)
        {
            try
            {
                DataSet ds = _dal.GetMember(sessionUserName, memberId);

                var member = Model.Member.FromDataRow(ds.Tables[0].Rows[0]);
                member.Country = Model.Country.FromDataRow(ds.Tables[1].Rows[0]);
                member.State = Model.State.FromDataRow(ds.Tables[2].Rows[0]);

                return member;
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

        public Model.MemberAccount GetMemberAccount(string sessionUserName, int memberId)
        {
            try
            {
                DataSet ds = _dal.GetMemberAccount(sessionUserName, memberId);

                return Model.MemberAccount.FromDataRow(ds.Tables[0].Rows[0]);
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

        public void OnNodeMemberAddFailure(string sessionUserName, int memberId)
        {
            _dal.OnNodeMemberAddFailure(sessionUserName, memberId);
        }
        public async Task<ErrorEx.Error> CreateMemberInNode(string adminUser, string adminPassword, Model.Member member, Model.MemberAccount memberAccount)
        {
            /*Irrespective of the Success or Failure of this Procedure. 
             * The User is informed that a temporary password has been sent to him/her
             * In case this process fails, the Service Broker Message Queue will ensure that
             * the Member is created and a temporary password is sent to the member.
             */
            ErrorEx.Error error = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", adminUser, adminPassword))));

                    client.DefaultRequestHeaders
                          .Accept
                          .Add(new MediaTypeWithQualityHeaderValue("application/json"));



                    var postData = new Dictionary<string, object>()
                    {
                        { "Member", member },
                        { "MemberAccount", memberAccount}
                    };

                    using (var response = await client.PostAsync(string.Format("{0}/api/adminmember/addnodemember", member.MemberHost), CHelper.HttpHelper.ConvertToHttpContent(postData)))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var resultJsonString = await response.Content.ReadAsStringAsync();

                            var result = JsonConvert.DeserializeObject<Model.WebAPI.WebAPIResult>(resultJsonString);
                            if (!result.Success)
                            {
                                error = JsonConvert.DeserializeObject<ErrorEx.Error>(result.Error.ToString());
                                CHelper.LogHelper.Error(error);
                                return error;
                            }

                            return null;
                        }
                        else
                        {
                            error = new ErrorEx.Error()
                            {
                                Code = 1,
                                Message = CHelper.HttpHelper.ReasonPhraseToMessage(response.ReasonPhrase),
                                ExceptionMessage = response.ReasonPhrase
                            };
                            CHelper.LogHelper.Error(response);

                            return error;
                        }
                    }
                }

            }
            catch (ErrorEx.OPUException ex)
            {
                return new ErrorEx.Error()
                {
                    Code = ex.ErrorCode,
                    Message = ex.ErrorMessage,
                    ExceptionMessage = ex.ExceptionMessage
                };
            }
            catch (Exception ex)
            {
                error = ErrorEx.Error.GetUnknownError(ex);
                CHelper.LogHelper.Error(error);
                return error;
            }
        }

        public void UpdateMember(string sessionUserName, Model.Member member)
        {
            _dal.UpdateMember(sessionUserName, member);
        }

        public Dictionary<string, object> GetMemberPreRequisites(string sessionUserName)
        {
            try
            {
                var ds = _dal.GetMemberPreRequisites(sessionUserName);

                var result = new Dictionary<string, object>();
                result.Add("TaxIdTypes", Model.TaxIdType.FromDataTable(ds.Tables[0]));
                result.Add("Countries", Model.Country.FromDataTable(ds.Tables[1]));
                result.Add("States", Model.State.FromDataTable(ds.Tables[2]));

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

        public int AddMember(string sessionUserName, Model.Member member, string otp)
        {
            try
            {
                if (!OTPHelper.ValidateRegistrationOTP(member.MemberEmailId, otp))
                {
                    throw new ErrorEx.OPUException(1, "Inavlid OTP", "Invalid OTP");
                }

                return _dal.AddMember(sessionUserName, member);
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

        public void SendRegistrationOTP(string sessionUserName, Model.Member member)
        {
            if (string.IsNullOrWhiteSpace(member.MemberEmailId)
                || (!CHelper.FormatValidationHelper.IsValidEmailId(member.MemberEmailId)))
                throw new Exception("Invalid Email Id");

            var email = new Email();
            email.SendEmail(sessionUserName, Common.Helper.Constants.SendEmailType.RegistrationOTP, member.MemberEmailId, OTPHelper.GenerateRegistrationOTP(member.MemberEmailId));
        }

        public string GetMemberHost(string sessionUserName, string memberEmailId)
        {
            if (string.IsNullOrWhiteSpace(memberEmailId)
                || (!CHelper.FormatValidationHelper.IsValidEmailId(memberEmailId)))
                throw new Exception("Invalid Email Id");

            try
            {
                return _dal.GetMemberHost(sessionUserName, memberEmailId);
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

        public Dictionary<string, object> GetOtherNodeMembers(string sessionUserName, OtherNodeMemberRequest request)
        {
            {
                try
                {
                    var ds = _dal.GetOtherNodeMembers(sessionUserName, request);

                    var result = new Dictionary<string, object>();
                    result.Add("Members", Model.Member.FromDataTable(ds.Tables[0]));
                    var supplierSettings = Model.SupplierSetting.FromDataTable(ds.Tables[1]);
                    var allSupplierSettingStates = Model.SupplierSettingState.FromDataTable(ds.Tables[2]);

                    foreach (var st in supplierSettings)
                    {
                        st.SupplierSettingStates = allSupplierSettingStates.Where(s => s.SupplierId == st.SupplierId).ToList();
                    }

                    result.Add("SupplierSettings", supplierSettings);

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

        public Model.InitiatedPayment CalculatePaymentInfo(string sessionUserName, Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                var ds = _dal.CalculatePaymentInfo(sessionUserName, initiatedPayment);

                return Model.InitiatedPayment.FromDataRow(ds.Tables[0].Rows[0]);
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

        public int AddInitiatedPayment(string sessionUserName, Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                return _dal.AddInitiatedPayment(sessionUserName, initiatedPayment);
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

        public void CompleteInitiatedPayment(string sessionUserName, Model.InitiatedPayment initiatedPayment, string PGTransactionReference, bool isDeclined)
        {
            try
            {
                _dal.CompleteInitiatedPayment(sessionUserName, initiatedPayment, PGTransactionReference, isDeclined);
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

        public Model.InitiatedPayment GetInitiatedPayment(string sessionUserName, Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                DataSet ds = _dal.GetInitiatedPayment(sessionUserName, initiatedPayment);

                return Model.InitiatedPayment.FromDataRow(ds.Tables[0].Rows[0]);
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

        public Model.InitiatedPayment GetInitiatedPaymentForPaymentGatewayProcessing(string sessionUserName, Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                DataSet ds = _dal.GetInitiatedPaymentForPaymentGatewayProcessing(sessionUserName, initiatedPayment);

                return Model.InitiatedPayment.FromDataRow(ds.Tables[0].Rows[0]);
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


        public Model.UpgradeLicense CalculateUpgradeInfo(string sessionUserName, Model.UpgradeLicense upgradeLicense)
        {
            try
            {
                var ds = _dal.CalculateUpgradeInfo(sessionUserName, upgradeLicense);
                return Model.UpgradeLicense.FromDataRow(ds.Tables[0].Rows[0]);
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

        public void UpgradeLicense(string sessionUserName, Model.UpgradeLicense upgradeLicense)
        {
            try
            {
                _dal.UpgradeLicense(sessionUserName, upgradeLicense);
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
