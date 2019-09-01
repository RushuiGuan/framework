using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.Core
{
    public class IntArray : UserDefinedTableType<int>
    {
        public IntArray(IEnumerable<int> data) : base(data) { }

        public override string CustomTypeName => "dbo.IntArray";

        protected override SqlDataRecord Build(int data)
        {
            var record = new SqlDataRecord(new SqlMetaData("Value", System.Data.SqlDbType.Int));
            record.SetInt32(0, data);
            return record;
        }
    }
}