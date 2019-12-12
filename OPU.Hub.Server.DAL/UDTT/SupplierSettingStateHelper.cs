using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using System.Data;

using Model = OPU.Common.Model;

namespace OPU.Hub.Server.DAL.UDTT
{
    internal class SupplierSettingStateHelper
    {
        private static SqlDataRecord ToSqlDataRecord(SqlMetaData[] sql, Model.SupplierSettingState model)
        {
            var record = new SqlDataRecord(sql);

            record.SetInt32(0, model.SupplierId);
            record.SetString(1, model.StateCode);
            record.SetString(2, model.CountryCode);

            return record;
        }

        public static IEnumerable<SqlDataRecord> ToSqlDataRecords(List<Model.SupplierSettingState> modelList)
        {
            var sql = new SqlMetaData[3];

            sql[0] = new SqlMetaData("SupplierId", SqlDbType.Int);
            sql[1] = new SqlMetaData("StateCode", SqlDbType.VarChar, Model.SupplierSettingState.FieldLength.StateCode);
            sql[2] = new SqlMetaData("CountryCode", SqlDbType.VarChar, Model.SupplierSettingState.FieldLength.CountryCode);

            var result = modelList.Select(model => ToSqlDataRecord(sql, model)).ToList();

            if (result.Count < 1)
            {
                return null;
            }

            return result;
        }
    }
}
