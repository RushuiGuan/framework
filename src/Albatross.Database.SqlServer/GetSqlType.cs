using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class GetSqlType : IGetSqlType {
		IGetDbConnection getDbConnection;

		public GetSqlType(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}


		public SqlType Get(Database database, string schema, string name) {
			if (string.IsNullOrEmpty(schema)) { schema = "sys"; }
			using (var db = getDbConnection.Get(database)) {
				return db.QueryFirst<SqlType>(GetCommand(schema, name));
			}
		}


		CommandDefinition GetCommand(string schema, string name) {
			return new CommandDefinition(@"
select 
	schemas.name as [Schema],
	types.Name, 
	types.max_length as MaxLength, 
	types.Precision,
	types.Scale,
	types.is_nullable as IsNullable,
	types.is_user_defined as IsUserDefined,
	types.is_table_type as IsTableType
from sys.types join sys.schemas on types.schema_id = schemas.schema_id
where schemas.name = @schema and types.name = @name
", new { schema = schema, name = name, });
		}
	}
}
