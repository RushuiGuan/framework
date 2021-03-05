using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class GetProcedure : IGetProcedure {
		IGetDbConnection getDbConnection;
		IListProcedureParameter listProcedureParameter;
		IGetProcedureDefinition getProcedureDefinition;
		IGetDatabasePermission getDatabasePermission;

		public GetProcedure(IGetDbConnection getDbConnection, IListProcedureParameter listProcedureParameter, IGetProcedureDefinition getProcedureDefinition, IGetDatabasePermission getDatabasePermission) {
			this.getDbConnection = getDbConnection;
			this.listProcedureParameter = listProcedureParameter;
			this.getProcedureDefinition = getProcedureDefinition;
			this.getDatabasePermission = getDatabasePermission;
		}

		public Procedure Get(Database database, string schema, string name) {
			Procedure procedure;
			using (var db = getDbConnection.Get(database)) {
				procedure = db.QueryFirst<Procedure>(GetCommandDefinition(schema, name));
			}
			procedure.Database = database;
			procedure.Parameters = listProcedureParameter.List(procedure);
			procedure.CreateScript = getProcedureDefinition.Get(procedure);
			procedure.Permissions = getDatabasePermission.Get(procedure);
			return procedure;
		}

		CommandDefinition GetCommandDefinition(string schema, string name) {
			return new CommandDefinition(@"
select 
	SPECIFIC_SCHEMA as [Schema],
	SPECIFIC_NAME as [Name],
	CREATED,
	LAST_ALTERED AS Modified
from INFORMATION_SCHEMA.ROUTINES 
where ROUTINE_TYPE = 'procedure' and SPECIFIC_SCHEMA = @schema and SPECIFIC_NAME = @name;
", new { schema = schema, name = name, });
		}
	}
}
