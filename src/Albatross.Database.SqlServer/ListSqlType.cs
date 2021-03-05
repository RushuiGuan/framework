using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class ListSqlType : IListSqlType {
		IGetDbConnection getDbConnection;

		public ListSqlType(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}


		public IEnumerable<SqlType> List(Database database) {
			using (var db = getDbConnection.Get(database)) {
				return db.Query<SqlType>(GetCommand());
			}
		}

		CommandDefinition GetCommand() {
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
");
		}
	}
}
