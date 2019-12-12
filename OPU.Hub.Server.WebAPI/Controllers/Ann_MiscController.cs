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
    [RoutePrefix("api/annMisc")]
    public class Ann_MiscController : ApiController
    {
        [HttpGet]
        [Route("payuri")]
        public IHttpActionResult GetPaymentURI()
        {
            try
            {
                object data = null;

                #region Custom Code

                data = Properties.Settings.Default.PaymentURI;

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
