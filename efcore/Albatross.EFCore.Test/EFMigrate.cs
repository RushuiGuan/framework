using System.Threading.Tasks;
using CommandLine;
using Albatross.EFCore.SqlServer;

namespace Albatross.EFCore.Test {
	[Verb("ef-migrate", HelpText = "Migrate database using dotnet ef tool")]
	public class EFMigrationOption { }

	public class EFMigrate : MyUtilityBase<EFMigrationOption> {
		public EFMigrate(EFMigrationOption option) : base(option) {
		}

		public async Task<int> RunUtility(SqlServerMigration<MySqlServerMigration> svc) {
			await svc.MigrateEfCore();
			return 0;
		}
	}
}
