using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Model = OPU.Common.Model;
using Helper = OPU.Common.Helper;
using SQLClientHelper = OPU.Common.Helper.SQLClient;
using ErrorEx = OPU.Common.ErrorAndException;

namespace OPU.Hub.Server.DAL
{
    public abstract class DALBase
    {
        protected SQLClientHelper.ParameterHelper __parameterHelper;
        protected SQLClientHelper.ParameterHelper _parameterHelper
        {
            get
            {
                return __parameterHelper ?? (__parameterHelper = new SQLClientHelper.ParameterHelper());
            }
        }

        protected SQLClientHelper.ErrorHelper __errorHandler;
        protected SQLClientHelper.ErrorHelper _errorHandler
        {
            get
            {
                return __errorHandler ?? (__errorHandler = new SQLClientHelper.ErrorHelper());
            }
        }

    }
}
