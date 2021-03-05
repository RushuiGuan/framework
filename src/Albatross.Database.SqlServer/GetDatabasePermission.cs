using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Database.SqlServer
{
	public class GetDatabasePermission : IGetDatabasePermission {
		IGetDbConnection getDbConnection;
		public GetDatabasePermission(IGetDbConnection getDbConnection) {
			this.getDbConnection = getDbConnection;
		}


		public IEnumerable<DatabasePermission> Get(IDatabaseObject @object) {
			using (var db = getDbConnection.Get(@object.Database)) {
				return db.Query<DatabasePermission>(@query, new { schema = @object.Schema, name = @object.Name });
			}
		}

		const string query = @"
SELECT 
	permission.state_desc as [State],
	permission.permission_name as Permission,
	principals.[name] as Principal
FROM sys.objects obj
join sys.schemas on schemas.schema_id = obj.schema_id
join sys.database_permissions permission ON permission.major_id = obj.object_id
join sys.database_principals principals on principals.principal_id = permission.grantee_principal_id
WHERE 
	permission.state IN ('G', 'W') -- GRANT or GRANT WITH GRANT
	and obj.name = @name and schemas.name = @schema
";
	}
}
