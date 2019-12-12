using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using OPU.Common.Helper;
using BL = OPU.Hub.Server.BL;
using Model = OPU.Common.Model;
using ErrAndEx = OPU.Common.ErrorAndException;
using System.Configuration;
using System.Threading.Tasks;
using OPU.Server.Helper;
using OPU.Common.Model.Composite;

namespace OPU.Hub.Server.WebAPI.Controllers
{
    [RoutePrefix("api/annMember")]
    public class Ann_MemberController : ApiController
    {
        BL.Member __blMember = null;
        private BL.Member _blMember
        {
            get
            {
                return __blMember ?? (__blMember = new BL.Member());
            }
        }

        [HttpGet]
        [Route("prerequisites")]
        public IHttpActionResult GetMemberPreRequisites()
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.GetMemberPreRequisites(Properties.Settings.Default.AdminUser);

                #endregion Custom Code

                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = true,
                    Data = data,
                    Error = null
                });
            }
            catch (ErrAndEx.OPUException ex)
            {
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = new ErrAndEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.ErrorMessage,
                        ExceptionMessage = ex.ExceptionMessage
                    }
                });
            }
            catch (Exception ex)
            {
                //Log Exception
                LogHelper.Error(ex);

                //return NotFound();
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = ErrAndEx.Error.GetUnknownError(ex)
                });
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMember([FromBody] Dictionary<string, object> input)
        {
            try
            {
                var member = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Member>(input["member"].ToString());
                var otp = input["otp"].ToString();

                object data = null;

                #region Custom Code
                member.MemberId = _blMember.AddMember(Properties.Settings.Default.AdminUser, member, otp);

                var adminUser = Properties.Settings.Default.AdminUser;

                try
                {
                    member = _blMember.GetMember(adminUser, member.MemberId);
                    var memberAccount = _blMember.GetMemberAccount(adminUser, member.MemberId);
                    var scheme = Properties.Settings.Default.AdminPassword2.Substring(10, 2);
                    var cryptoHelper = new CryptoHelper(scheme);
                    var adminPassword = cryptoHelper.Decrypt(Properties.Settings.Default.AdminPassword);

                    var error = await _blMember.CreateMemberInNode(adminUser, adminPassword, member, memberAccount);

                    if (error != null)
                    {
                        //Send Add Node Member to Service Broker Queue
                        try
                        {
                            _blMember.OnNodeMemberAddFailure(adminUser, member.MemberId);
                        }
                        catch
                        {
                            /*Ignore the Exception and show Temporary Password message sent message.
                            The Manual Admin process will be used in this case to create the Member 
                            in the Node*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is ErrAndEx.OPUException))
                    {
                        LogHelper.Error(ex);
                    }

                    try
                    {
                        _blMember.OnNodeMemberAddFailure(adminUser, member.MemberId);
                    }
                    catch
                    {
                        /*Ignore the Exception and show Temporary Password message sent message.
                        The Manual Admin process will be used in this case to create the Member 
                        in the Node*/
                    }
                }
                #endregion Custom Code

                data = member;

                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = true,
                    Data = data,
                    Error = null
                });
            }
            catch (ErrAndEx.OPUException ex)
            {
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = new ErrAndEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.ErrorMessage,
                        ExceptionMessage = ex.ExceptionMessage
                    }
                });
            }
            catch (Exception ex)
            {
                //Log Exception
                LogHelper.Error(ex);

                //return NotFound();
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = ErrAndEx.Error.GetUnknownError(ex)
                });
            }
        }

        [Route("sendregistrationotp")]
        [HttpPost]
        public IHttpActionResult SendRegistrationOTP([FromBody] Model.Member member)
        {
            try
            {
                object data = null;

                #region Custom Code

                _blMember.SendRegistrationOTP(Properties.Settings.Default.AdminUser, member);

                #endregion Custom Code

                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = true,
                    Data = data,
                    Error = null
                });
            }
            catch (ErrAndEx.OPUException ex)
            {
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = new ErrAndEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.ErrorMessage,
                        ExceptionMessage = ex.ExceptionMessage
                    }
                });
            }
            catch (Exception ex)
            {
                //Log Exception
                LogHelper.Error(ex);

                //return NotFound();
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = ErrAndEx.Error.GetUnknownError(ex)
                });
            }
        }

        [Route("getmemberhost")]
        [HttpPost]
        public IHttpActionResult GetMemberHost([FromBody] string memberEmailId)
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.GetMemberHost(Properties.Settings.Default.AdminUser, memberEmailId);

                #endregion Custom Code

                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = true,
                    Data = data,
                    Error = null
                });
            }
            catch (ErrAndEx.OPUException ex)
            {
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = new ErrAndEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.ErrorMessage,
                        ExceptionMessage = ex.ExceptionMessage
                    }
                });
            }
            catch (Exception ex)
            {
                //Log Exception
                LogHelper.Error(ex);

                //return NotFound();
                return Ok(new Model.WebAPI.WebAPIResult()
                {
                    Success = false,
                    Data = null,
                    Error = ErrAndEx.Error.GetUnknownError(ex)
                });
            }
        }

    }
}
