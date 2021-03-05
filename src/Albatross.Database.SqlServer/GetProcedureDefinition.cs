using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class GetProcedureDefinition : IGetProcedureDefinition {
		IGetDbConnection getDbConnection;
		public GetProcedureDefinition(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public string Get(Procedure procedure) {
			using (var db = getDbConnection.Get(procedure.Database)) {
				return db.QueryFirst<string>(query, new { name = procedure.Name, schema = procedure.Schema });
			}
		}

		const string query = @"
select OBJECT_DEFINITION(procedures.object_id) as Definition
 from sys.procedures
 join sys.schemas on procedures.schema_id = schemas.schema_id
 where procedures.name = @name and schemas.name = @schema
";
	}
}
