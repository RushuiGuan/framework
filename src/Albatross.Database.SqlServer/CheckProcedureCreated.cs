using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class CheckProcedureCreated : ICheckProcedureCreated {
		IGetDbConnection getDbConnection;
		public CheckProcedureCreated(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public bool Check(Procedure procedure) {
			using (var db = getDbConnection.Get(procedure.Database)) {
				return db.ExecuteScalar<bool>(query, new { schema = procedure.Schema, name = procedure.Name });
			}
		}


		const string query = @"
if exists (
	 select * from INFORMATION_SCHEMA.ROUTINES 
	 where ROUTINE_NAME = @name and ROUTINE_SCHEMA = @schema and ROUTINE_TYPE = 'procedure'
) begin
	select cast(1 as bit) as Flag
end else begin
	select cast(0 as bit) as Flag
end;";
	}
}
