using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPU.Hub.Server.BL
{
    public class Email
    {
        private DAL.Email __dal;
        private DAL.Email _dal
        {
            get
            {
                return __dal ?? (__dal = new DAL.Email());
            }
        }


        public void SendEmail(string sessionUserName, string emailType, string emailId, string messageParams)
        {
            _dal.SendEmail(sessionUserName, emailType, emailId, messageParams);
        }

        public Tuple<int, int> Sync_CheckEmailFailure(SqlConnection cnn, string sessionUserName, string emailId, string emailType, string message)
        {
            DataSet ds = _dal.Sync_CheckEmailFailure(cnn, sessionUserName, emailId, emailType, message);

            var failureCount = (int)ds.Tables[0].Rows[0]["FailureCount"];
            var minutesSinceLastFailed = (int)ds.Tables[0].Rows[0]["MinutesSinceLastFailed"];

            return new Tuple<int, int>
            (
                failureCount,
                minutesSinceLastFailed
            );
        }
    }
}
