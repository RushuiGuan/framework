using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.Core
{
    public class TextArray : UserDefinedTableType<string>
    {
        public TextArray(IEnumerable<string> data) : base(data) { }
        public override string CustomTypeName => "dbo.TextArray";
        protected override SqlDataRecord Build(string data)
        {
            var record = new SqlDataRecord(new SqlMetaData("Value", System.Data.SqlDbType.NVarChar, -1));
            record.SetString(0, data);
            return record;
        }
    }
}