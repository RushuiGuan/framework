using System.Collections.Generic;
using Albatross.Database;
using Dapper;

namespace Albatross.Database.SqlServer {
	public class ListTableIndexColumn : IListTableIndexColumn{
		IGetDbConnection getDbConnection;

		public ListTableIndexColumn(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public IEnumerable<IndexColumn> List(Index index) {
			using (var db = getDbConnection.Get(index.Table.Database)) {
				return db.Query<IndexColumn>(GetCommand(index.Table.Schema, index.Table.Name, index.Name));
			}
		}

		CommandDefinition GetCommand(string schema, string table, string indexName) {
			return new CommandDefinition(@"
select 
	indexcolumns.key_ordinal as KeyOrdinal,
	indexcolumns.is_descending_key as IsDescending,
	columns.name as [Name]
from sys.indexes 
join sys.index_columns indexcolumns on indexes.index_id = indexcolumns.index_id and indexes.object_id = indexcolumns.object_id
join sys.tables on tables.object_id = indexes.object_id
join sys.schemas on schemas.schema_id = tables.schema_id
join sys.columns on columns.object_id = tables.object_id and indexcolumns.column_id = columns.column_id
where tables.name = @table and schemas.name = @schema and indexes.name = @indexName
order by indexcolumns.key_ordinal
", new { schema = schema, table = table, indexName = indexName });
		}
	}
}
