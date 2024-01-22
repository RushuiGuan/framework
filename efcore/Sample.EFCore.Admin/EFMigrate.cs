using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Albatross.Config;
using Albatross.EFCore.SqlServer;
using Albatross.Hosting.Utility;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("ef-migrate", HelpText = "Migrate database using dotnet ef tool")]
	public class EFMigrationOption:BaseOption { }

	public class EFMigrate : MyUtilityBase<EFMigrationOption> {
		public EFMigrate(EFMigrationOption option) : base(option) {
		}

		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
			services.AddScoped<SqlServerMigration<SampleSqlServerMigration>>();
			services.AddScoped<ISqlBatchExecution, SqlBatchExecution>();
			services.AddScoped(provider => {
				var config = provider.GetRequiredService<SampleConfig>();
				return new SampleSqlServerMigration(config.ConnectionString);
			});
		}

		public async Task<int> RunUtility(SqlServerMigration<SampleSqlServerMigration> svc) {
			await svc.MigrateEfCore();
			return 0;
		}
	}
}
