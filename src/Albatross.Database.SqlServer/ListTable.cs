using System;
using System.Collections.Generic;
using System.Linq;
using Albatross.Database;
using Dapper;

namespace Albatross.Database.SqlServer {
	public class ListTable : IListTable {
		IGetDbConnection getDbConnection;
		IListTableIndex listTableIndex;
		IListTableColumn listTableColumn;
		IParseCriteria parseCriteria;

		public ListTable(IGetDbConnection getDbConnection, IListTableColumn listTableColumn, IListTableIndex listTableIndex, IParseCriteria parseCriteria) {
			this.getDbConnection = getDbConnection;
			this.listTableIndex = listTableIndex;
			this.listTableColumn = listTableColumn;
			this.parseCriteria = parseCriteria;
		}

		public IEnumerable<Table> Get(Database database, string criteria) {
			IEnumerable<Table> tables;
			string schema, name;
			parseCriteria.Parse(criteria, out schema, out name);
			using (var db = getDbConnection.Get(database)) {
				tables = db.Query<Table>(Get(schema, name));
			}
			foreach (var table in tables) {
				table.Database = database;
				table.Columns = listTableColumn.List(table);
				table.IdentityColumn = (from item in table.Columns where item.IsIdentity select item).FirstOrDefault();
				var indexes = listTableIndex.List(table);
				table.PrimaryKeys = (from index in indexes where index.IsPrimaryKey select index).FirstOrDefault()?.Columns;
			}
			return tables;
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
where (@table is null or tables.name like @table) and (@schema is null or schemas.name like @schema)", new { schema = schema, table = name});
		}
	}
}
