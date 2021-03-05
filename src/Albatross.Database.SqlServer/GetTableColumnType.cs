using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class GetTableColumnType : IGetTableColumnType {
		IGetDbConnection getDbConnection;

		public GetTableColumnType(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public SqlType Get(Table table, string column) {
			using (var db = getDbConnection.Get(table.Database)) {
				return db.QueryFirst<SqlType>(GetCommand(table.Schema, table.Name, column));
			}
		}

		CommandDefinition GetCommand(string schema, string table, string column) {
			return new CommandDefinition(@"
select  
	columns.name as ColumnName,
	typeSchema.name as [Schema],
	types.name,
	case when types.name in ('nchar', 'ntext', 'nvarchar') and columns.max_length <> -1 then columns.max_length / 2 else columns.max_length end as MaxLength,
	types.precision,
	types.scale,
	columns.is_nullable as IsNullable,
	types.is_user_defined as IsUserDefined,
	types.is_table_type as IsTableType
from sys.columns 
join sys.tables on columns.object_id = tables.object_id
join sys.schemas on tables.schema_id = schemas.schema_id
join sys.types on types.user_type_id = columns.user_type_id and types.system_type_id = columns.system_type_id
join sys.schemas typeSchema on typeSchema.schema_id = types.schema_id
where tables.name = @table and schemas.name = @schema and (columns.name = @columnName or @columnName is null)
", new { schema = schema, table = table, columnName = column });
		}
	}
}
