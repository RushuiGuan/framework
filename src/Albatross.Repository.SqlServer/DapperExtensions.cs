using Albatross.Repository.SqlServer;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Albatross.Repository.SqlServer {
	public static class DapperExtensions {
		public static SqlMapper.ICustomQueryParameter AsTableValuedParameter(this IEnumerable<int> values) {
			DataTable table = new DataTable();
			table.Columns.Add("Value", typeof(int));
			foreach (var item in values) {
				table.Rows.Add(item);
			}
			return table.AsTableValuedParameter("dbo.IntArray");
		}

		public static SqlMapper.ICustomQueryParameter AsTableValuedParameter(this IEnumerable<string> values) {
			DataTable table = new DataTable();
			table.Columns.Add("Value", typeof(string));
			foreach (var item in values) {
				table.Rows.Add(item);
			}
			return table.AsTableValuedParameter("dbo.StringArray");
		}
	}
}