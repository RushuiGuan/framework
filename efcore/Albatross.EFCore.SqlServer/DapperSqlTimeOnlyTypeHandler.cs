using Dapper;
using System;
using System.Data;

namespace Albatross.EFCore.SqlServer {
	public class DapperSqlTimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly> {
		public override void SetValue(IDbDataParameter parameter, TimeOnly time) {
			parameter.Value = time.ToString();
		}

		public override TimeOnly Parse(object value) {
			return TimeOnly.FromTimeSpan((TimeSpan)value);
		}
	}
}