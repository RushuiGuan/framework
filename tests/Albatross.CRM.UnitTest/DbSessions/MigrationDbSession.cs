using Albatross.CRM.Repository;
using Albatross.Repository.ByEFCore;
using sql = Albatross.Repository.SqlServer;
using postgres = Albatross.Repository.PostgreSQL;

namespace Albatross.CRM.UnitTest.DbSessions {
	public class CRMDbSqlMigrationSession : CRMDbSession {
		public CRMDbSqlMigrationSession() : this(DbSession.Any) { }
		public CRMDbSqlMigrationSession(string connectionString) : base(sql.ServiceExtension.BuildMigrationOption<CRMDbSession>(CRMConstant.Schema, connectionString)) {
		}
	}

	public class CRMDbPostgresMigrationSession : CRMDbSession {
		public CRMDbPostgresMigrationSession() : this(DbSession.Any) { }
		public CRMDbPostgresMigrationSession(string connectionString) : base(postgres.ServiceExtension.BuildMigrationOption<CRMDbSession>(CRMConstant.Schema, connectionString)) {
		}
	}
}