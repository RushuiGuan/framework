using Sample.EFCore.Models;
using System.Reflection;
using Sql = Albatross.EFCore.SqlServer;


namespace Sample.EFCore.Admin {
	public class SampleSqlServerMigration : SampleDbSession {
		public SampleSqlServerMigration() : this("any") { }
		public SampleSqlServerMigration(string connectionString)
			: base(Sql.Extensions.BuildMigrationOption<SampleDbSession>(My.Schema.Sample, connectionString)) {
		}
	}
}