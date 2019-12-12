using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data;

using Model = OPU.Common.Model;

namespace OPU.Hub.Server.DAL.Helper.UDTT
{
    internal class MasterDetailHelper
    {
        private static SqlDataRecord ToSqlDataRecord(SqlMetaData[] sql, Model.MasterDetail value)
        {
            var record = new SqlDataRecord(sql);
            record.SetInt32(0, value.MasterId);
            record.SetInt32(1, value.DetailId);
            return record;
        }

        public static IEnumerable<SqlDataRecord> ToSqlDataRecords(List<Model.MasterDetail> values)
        {
            var sql = new SqlMetaData[2];
            sql[0] = new SqlMetaData("MasterId", SqlDbType.Int);
            sql[1] = new SqlMetaData("DetailId", SqlDbType.Int);

            var result = values.Select(v => ToSqlDataRecord(sql, v)).ToList();

            if (result.Count < 1)
            {
                return null;
            }

            return result;
        }

    }
}
