using System.Collections.Generic;
using Albatross.Database;
using Dapper;

namespace Albatross.Database.SqlServer {
	public class ListTableColumn : IListTableColumn {
		IGetDbConnection getDbConnection;
		IGetTableColumnType getTableColumnType;

		public ListTableColumn(IGetDbConnection getDbConnection,IGetTableColumnType getTableColumnType) {
			this.getDbConnection = getDbConnection;
			this.getTableColumnType = getTableColumnType;
		}

		public IEnumerable<Column> List(Table table) {
			IEnumerable<Column> columns;
			using (var db = getDbConnection.Get(table.Database)) {
				columns = db.Query<Column>(GetColumn(table.Schema, table.Name));
			}

			foreach (var column in columns) {
				column.Type = getTableColumnType.Get(table, column.Name);
			}

			return columns;
		}

		CommandDefinition GetColumn(string schema, string table) {
			return new CommandDefinition(@"
select  
	c.name as Name,
	c.is_nullable as IsNullable,
	c.is_computed as IsComputed,
	c.is_identity as IsIdentity,
	c.is_filestream as IsFileStream,
	c.column_id as OrdinalPosition
from sys.columns c
join sys.tables t on c.object_id = t.object_id
join sys.schemas s on s.schema_id = t.schema_id
join sys.types on types.user_type_id = c.user_type_id and types.system_type_id = c.system_type_id
where t.name = @name and s.name = @schema
", new { schema = schema, name = table, });
		}
	}
}
