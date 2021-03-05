using System.Linq;
using Albatross.Database;
using Dapper;

namespace Albatross.Database.SqlServer {
	public class GetTable : IGetTable {
		IGetDbConnection getDbConnection;
		IListTableIndex listTableIndex;
		IListTableColumn listTableColumn;

		public GetTable(IGetDbConnection getDbConnection, IListTableColumn listTableColumn, IListTableIndex listTableIndex) {
			this.getDbConnection = getDbConnection;
			this.listTableIndex = listTableIndex;
			this.listTableColumn = listTableColumn;
		}

		public Table Get(Database database, string schema, string name) {
			Table table;
			using (var db = getDbConnection.Get(database)) {
				table = db.QueryFirst<Table>(Get(schema, name));
			}
			table.Database = database;
			table.Columns = listTableColumn.List(table);
			table.IdentityColumn = (from item in table.Columns where item.IsIdentity select item).FirstOrDefault();
			var indexes = listTableIndex.List(table);
			table.PrimaryKeys = (from index in indexes where index.IsPrimaryKey select index).FirstOrDefault()?.Columns;
			return table;
		}

		CommandDefinition Get(string schema, string name) {
			return new CommandDefinition(@"
select  
	tables.name as Name, 
	schemas.name as [Schema],
	tables.create_date as Created,
	tables.modify_date as Modified
from sys.tables
join sys.schemas on tables.schema_id = schemas.schema_id
where tables.name = @table and schemas.name = @schema", new { schema = schema, table = name});
		}
	}
}
