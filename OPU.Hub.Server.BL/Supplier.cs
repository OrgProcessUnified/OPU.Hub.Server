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
    public class Supplier
    {
        private DAL.Supplier __dal;
        private DAL.Supplier _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.Supplier());
            }
        }

        public void UpdateSupplierSetting(string sessionUserName, Model.SupplierSetting model)
        {
            try
            {
                _dal.UpdateSupplierSetting(sessionUserName, model);
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
