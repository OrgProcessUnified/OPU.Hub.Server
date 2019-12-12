using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Model = OPU.Common.Model;
using ErrAndEx = OPU.Common.ErrorAndException;
using OPU.Common.Helper;


namespace OPU.Hub.Server.WebAPI.Controllers
{
    [RoutePrefix("api/adminuser")]
    [AdminBasicAuthentication]
    public class AdminUserController : ApiController
    {
        [HttpGet]
        [Route("login")]
        public IHttpActionResult AuthentiateUser()
        {
            try
            {
                object data = null;

                #region Custom Code
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
