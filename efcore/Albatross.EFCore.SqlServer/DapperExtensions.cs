using Albatross.EFCore.SqlServer;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Albatross.EFCore.SqlServer {
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
		public static void UseDateOnlyConversion() {
			SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
		}
		public static void UseTimeOnlyConversion() {
			SqlMapper.AddTypeHandler(new DapperSqlTimeOnlyTypeHandler());
		}
	}
}