
namespace Albatross.Repository.Test {
	public class MySqlServerMigration : MyDbSession {
		public MySqlServerMigration() : this("any") { }
		public MySqlServerMigration(string connectionString) : base(SqlServer.Extensions.BuildMigrationOption<MyDbSession>(Constant.Schema, connectionString)) {
		}
	}
}
