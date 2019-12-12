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
    [RoutePrefix("api/member")]
    [AdminBasicAuthentication]
    public class MemberController : ApiController
    {
        BL.Member __blMember = null;
        private BL.Member _blMember
        {
            get
            {
                return __blMember ?? (__blMember = new BL.Member());
            }
        }

        [Route("getothernodemembers")]
        [HttpPost]
        public IHttpActionResult GetOtherNodeMembers([FromBody] OtherNodeMemberRequest request)
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.GetOtherNodeMembers(Properties.Settings.Default.AdminUser, request);

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

        [Route("updatemember")]
        [HttpPost]
        public IHttpActionResult UpdateMember([FromBody] Model.Member member)
        {
            try
            {
                object data = null;

                #region Custom Code

                _blMember.UpdateMember(Properties.Settings.Default.AdminUser, member);

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

        [Route("paymentinfo")]
        [HttpPost]
        public IHttpActionResult CalculatePaymentInfo([FromBody] Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.CalculatePaymentInfo(Properties.Settings.Default.AdminUser, initiatedPayment);

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

        [Route("initiatepayment")]
        [HttpPost]
        public IHttpActionResult InitiatePayment([FromBody] Model.InitiatedPayment initiatedPayment)
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.AddInitiatedPayment(Properties.Settings.Default.AdminUser, initiatedPayment);

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

        [Route("upgradeinfo")]
        [HttpPost]
        public IHttpActionResult CalculateUpgradeInfo([FromBody] Model.UpgradeLicense upgradeLicense)
        {
            try
            {
                object data = null;

                #region Custom Code

                data = _blMember.CalculateUpgradeInfo(Properties.Settings.Default.AdminUser, upgradeLicense);

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

        [Route("upgradelicense")]
        [HttpPost]
        public IHttpActionResult UpgradeLicense([FromBody] Model.UpgradeLicense upgradeLicense)
        {
            try
            {
                object data = null;

                #region Custom Code

                _blMember.UpgradeLicense(Properties.Settings.Default.AdminUser, upgradeLicense);

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
