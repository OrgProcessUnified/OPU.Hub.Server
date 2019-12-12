using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data;

namespace OPU.Hub.Server.DAL.Helper.UDTT
{
    internal class IdHelper
    {
        private static SqlDataRecord ToSqlDataRecord(SqlMetaData[] sql, int value)
        {
            var record = new SqlDataRecord(sql);
            record.SetInt32(0, value);
            return record;
        }

        public static IEnumerable<SqlDataRecord> ToSqlDataRecords(List<int> ids)
        {
            var sql = new SqlMetaData[1];
            sql[0] = new SqlMetaData("Id", SqlDbType.Int);

            var result = ids.Select(i => ToSqlDataRecord(sql, i)).ToList();

            if (result.Count < 1)
            {
                return null;
            }

            return result;
        }
    }
}
