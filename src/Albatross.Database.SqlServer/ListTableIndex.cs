using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer {
	public class ListTableIndex : IListTableIndex {
		IGetDbConnection getDbConnection;
		IListTableIndexColumn listTableIndexColumn;

		public ListTableIndex(IGetDbConnection getDbConnection, IListTableIndexColumn listTableIndexColumn) {
			this.getDbConnection = getDbConnection;
			this.listTableIndexColumn = listTableIndexColumn;
		}

		public IEnumerable<Index> List(Table table) {
			IEnumerable<Index> items;
			using (var db = getDbConnection.Get(table.Database)) {
				items = db.Query<Index>(GetCommand(table.Schema, table.Name));
			}
			foreach (var item in items) {
				item.Table = table;
				item.Columns = listTableIndexColumn.List(item);
			}
			return items;
		}

		CommandDefinition GetCommand(string schema, string table) {
			return new CommandDefinition(@"
select 
	indexes.name,
	indexes.type,
	indexes.type_desc as TypeDesc,
	indexes.is_disabled as IsDisabled,
	indexes.is_unique as IsUnique,
	indexes.is_primary_key as IsPrimaryKey,
	indexes.is_unique_constraint as IsUniqueConstraint
from sys.indexes 
join sys.tables on tables.object_id = indexes.object_id
join sys.schemas on schemas.schema_id = tables.schema_id
where tables.name = @table and schemas.name = @schema
", new { schema = schema, table = table, });
		}
	}
}
