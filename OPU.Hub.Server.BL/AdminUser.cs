using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using ErrorEx = OPU.Common.ErrorAndException;
using CHelper = OPU.Common.Helper;
using Model = OPU.Common.Model;
using OPU.Hub.Server.DAL;
using OPU.Server.Helper;

namespace OPU.Hub.Server.BL
{
    public class AdminUser
    {
        private DAL.AdminUser __dal;
        private DAL.AdminUser _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.AdminUser());
            }
        }

        private Model.AdminUser GetUserForAuthentication(Model.AdminUser user)
        {
            try
            {
                var ds = _dal.GetUserForAuthentication(user);
                var users = Model.AdminUser.FromDataSet(ds);

                if (users.Count == 0)
                {
                    return null;
                }

                return users[0];
            }
            catch (Exception ex)
            {
                //Log Ex
                CHelper.LogHelper.Error(ex);
                return null;
            }
        }

        public ErrorEx.Error ValidateUser(Model.AdminUser user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.EmailId) || string.IsNullOrWhiteSpace(user.Password))
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.EMAIL_AND_PASSWORD_REQUIRED,
                        Message = Model.AdminUser.ErrorMessage.EMAIL_AND_PASSWORD_REQUIRED
                    };
                }

                var dbUser = GetUserForAuthentication(user);

                if (dbUser == null)
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.INVALID_EMAIL_ID_OR_PASSWORD,
                        Message = Model.AdminUser.ErrorMessage.INVALID_EMAIL_ID_OR_PASSWORD
                    };
                }

                if (string.IsNullOrWhiteSpace(dbUser.PasswordHash))
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.EMAIL_ID_NOT_SET_UP,
                        Message = Model.AdminUser.ErrorMessage.EMAIL_ID_NOT_SET_UP
                    };
                }

                var cryptoHelper = new CryptoHelper();
                if (!cryptoHelper.ValidateHash(user.Password, dbUser.PasswordHash))
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.INVALID_EMAIL_ID_OR_PASSWORD,
                        Message = Model.AdminUser.ErrorMessage.INVALID_EMAIL_ID_OR_PASSWORD
                    };
                }

                if (!dbUser.IsActive)
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.USER_NOTACTIVE,
                        Message = Model.AdminUser.ErrorMessage.USER_NOTACTIVE
                    };
                }

                if (dbUser.IsTempPassword)
                {
                    return new ErrorEx.Error()
                    {
                        Code = Model.AdminUser.ErrorCode.TEMP_PASSWORD_FORCE_CHANGE,
                        Message = Model.AdminUser.ErrorMessage.TEMP_PASSWORD_FORCE_CHANGE
                    };
                }

                return new ErrorEx.Error();
            }
            catch (Exception ex)
            {
                CHelper.LogHelper.Error(ex);
                return new ErrorEx.Error()
                {
                    Code = ErrorEx.Error.ErrorCode.UNKNOWN_ERROR,
                    Message = ErrorEx.Error.ErrorMesage.UNKNOWN_ERROR,
                    ExceptionMessage = ex.Message
                };
            }
        }

        public List<Model.AdminUser> GetUsers(string sessionUserName)
        {
            try
            {
                var ds = _dal.GetUsers(sessionUserName);

                return Model.AdminUser.FromDataSet(ds);
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

        public Dictionary<string, object> GetUserToEdit(string sessionUserName, int userId, bool getPreRequisites)
        {
            try
            {
                var ds = _dal.GetUserToEdit(sessionUserName, userId, getPreRequisites);

                var user = Model.AdminUser.FromDataRow(ds.Tables[0].Rows[0]);
                user.Roles = Model.AdminRole.FromDataTable(ds.Tables[1]);

                var result = new Dictionary<string, object>();
                result.Add("AdminUser", user);

                if (getPreRequisites)
                {
                    result.Add("Roles", Model.AdminRole.FromDataTable(ds.Tables[2]));
                    result.Add("Countries", Model.Country.FromDataTable(ds.Tables[3]));
                    result.Add("States", Model.State.FromDataTable(ds.Tables[4]));
                }
                else
                {
                    result.Add("Roles", new List<Model.AdminRole>());
                    result.Add("Countries", new List<Model.Country>());
                    result.Add("States", new List<Model.State>());
                }

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

        public Dictionary<string, object> GetUserPreRequisites(string sessionUserName)
        {
            try
            {
                var ds = _dal.GetUserPreRequisites(sessionUserName);

                var result = new Dictionary<string, object>();
                result.Add("Roles", Model.AdminRole.FromDataTable(ds.Tables[0]));
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

        public object SaveUser(string sessionUserName, Model.AdminUser user, bool readBack)
        {
            var saveSuccess = false;
            try
            {
                int userId = user.UserId;
                if (user.UserId <= 0) /*Add AdminUser*/
                {
                    userId = _dal.AddUser(sessionUserName, user);
                }
                else /*Update AdminUser*/
                {
                    _dal.UpateUser(sessionUserName, user);
                }

                saveSuccess = true;
                if (readBack)
                {
                    return GetUserToEdit(sessionUserName, userId, true);
                }

                return userId;
            }
            catch (ErrorEx.OPUException ex)
            {
                if (saveSuccess)
                {
                    return new ErrorEx.Error()
                    {
                        Code = ex.ErrorCode,
                        Message = ex.Message,
                        ExceptionMessage = ex.ExceptionMessage
                    };
                }
                throw ex;
            }
            catch (Exception ex)
            {
                CHelper.LogHelper.Error(ex);

                if (saveSuccess)
                {
                    return ErrorEx.Error.GetUnknownError(ex);
                }

                throw new ErrorEx.OPUException(ex);
            }

        }

        public void DeleteUser(string sessionUserName, int userId)
        {
            try
            {
                _dal.DeleteUser(sessionUserName, userId);
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

        public void ActivateUser(string sessionUserName, int userId)
        {
            try
            {
                _dal.ActivateUser(sessionUserName, userId);
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

        public void DeactivateUser(string sessionUserName, int userId)
        {
            try
            {
                _dal.DeactivateUser(sessionUserName, userId);
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
