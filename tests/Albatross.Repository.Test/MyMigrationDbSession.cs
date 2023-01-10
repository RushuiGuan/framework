using Albatross.Repository.SqlServer;

namespace Albatross.Repository.Test {
	public class MySqlServerMigration : MyDbSession {
		public MySqlServerMigration() : this("any") { }
		public MySqlServerMigration(string connectionString) : base(ServiceExtension.BuildMigrationOption<MyDbSession>(Constant.Schema, connectionString)) {
		}
	}
}
