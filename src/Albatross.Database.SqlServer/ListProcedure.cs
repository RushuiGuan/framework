using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class ListProcedure : IListProcedure {
		IGetDbConnection getDbConnection;
		IListProcedureParameter listProcedureParameter;
		IParseCriteria parseCriteria;

		public ListProcedure(IGetDbConnection getDbConnection, IListProcedureParameter listProcedureParameter, IParseCriteria parseCriteria) {
			this.getDbConnection = getDbConnection;
			this.listProcedureParameter = listProcedureParameter;
			this.parseCriteria = parseCriteria;
		}

		public IEnumerable<Procedure> Get(Database database, string criteria) {
			string name, schema;
			parseCriteria.Parse(criteria, out schema, out name);
			IEnumerable<Procedure> procedures;
			using (var db = getDbConnection.Get(database)) {
				procedures = db.Query<Procedure>(GetCommandDefinition(schema, name));
			}
			foreach (var item in procedures) {
				item.Database = database;
				item.Parameters = listProcedureParameter.List(item);
			}
			return procedures;
		}

		CommandDefinition GetCommandDefinition(string schema, string name) {
			return new CommandDefinition(@"
select 
	SPECIFIC_SCHEMA as [Schema],
	SPECIFIC_NAME as [Name],
	CREATED,
	LAST_ALTERED AS Modified
from INFORMATION_SCHEMA.ROUTINES 
where ROUTINE_TYPE = 'procedure' and (@schema is null or SPECIFIC_SCHEMA like @schema) and (@name is null or SPECIFIC_NAME like @name);
", new { schema = schema, name = name, });
		}
	}
}
