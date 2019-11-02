using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using sql = Albatross.Repository.SqlServer;
using postgres = Albatross.Repository.PostgreSQL;

namespace Albatross.Repository.UnitTest {
	public class CRMDbSession : DbSession {
		public CRMDbSession(DbContextOptions<CRMDbSession> option) : base(option) { }
	}

	public class CRMDbSqlMigrationSession : CRMDbSession {
		public CRMDbSqlMigrationSession() : this("any") { }
		public CRMDbSqlMigrationSession(string connectionString) : base(sql.ServiceExtension.BuildMigrationOption<CRMDbSession>(Constant.Schema, connectionString)) {
		}
	}

	public class CRMDbPostgresMigrationSession : CRMDbSession {
		public CRMDbPostgresMigrationSession() : this("any") { }
		public CRMDbPostgresMigrationSession(string connectionString) : base(postgres.ServiceExtension.BuildMigrationOption<CRMDbSession>(Constant.Schema, connectionString)) {
		}
	}
}