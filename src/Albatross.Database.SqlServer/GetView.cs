using System.Linq;
using Albatross.Database;
using Dapper;

namespace Albatross.Database.SqlServer {
	public class GetView : IGetView {
		IGetDbConnection getDbConnection;

		public GetView(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}

		public View Get(Database database, string schema, string name) {
			View table;
			using (var db = getDbConnection.Get(database)) {
				table = db.QueryFirst<View>(Get(schema, name));
			}
			table.Database = database;
			return table;
		}

		CommandDefinition Get(string schema, string name) {
			return new CommandDefinition(@"
select  
	views.name as Name, 
	schemas.name as [Schema],
	views.create_date as Created,
	views.modify_date as Modified
from sys.views
join sys.schemas on views.schema_id = schemas.schema_id
where views.name = @view and schemas.name = @schema", new { schema = schema, view = name});
		}
	}
}
