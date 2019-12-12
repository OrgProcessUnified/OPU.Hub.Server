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

namespace OPU.Hub.Server.BL
{
    public class User
    {
        private DAL.User __dal;
        private DAL.User _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.User());
            }
        }

        public void SyncUser(string sessionUserName, string oldANdNewUserEmailIdsCSV)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldANdNewUserEmailIdsCSV))
                {
                    return;
                }

                string oldUserEmailId = string.Empty;
                string newUserEmailId = string.Empty;

                var emails = oldANdNewUserEmailIdsCSV.Split(",".ToCharArray(), StringSplitOptions.None).ToList();
                if ((emails.Count != 2)
                    || (!emails.Any(e => !string.IsNullOrWhiteSpace(e))))
                {
                        return;
                }

                oldUserEmailId = emails[0];
                newUserEmailId = emails[1];

                _dal.SyncUser(sessionUserName, oldUserEmailId, newUserEmailId);
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
