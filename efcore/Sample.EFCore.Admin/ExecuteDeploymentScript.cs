using Albatross.Config;
using Albatross.Hosting.Utility;
using Albatross.EFCore.SqlServer;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	[Verb("exec-script", HelpText = "Execute deployment scripts")]
	public class ExecuteDeploymentScriptOption : BaseOption {
		[Option('l', "location")]
		public string? Location { get; set; }
	}

	public class ExecuteDeploymentScript : MyUtilityBase<ExecuteDeploymentScriptOption> {
		public ExecuteDeploymentScript(ExecuteDeploymentScriptOption option) : base(option) {
		}
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddScoped<SqlServerMigration<SampleSqlServerMigration>>();
			services.AddScoped<SampleSqlServerMigration>(provider => {
				var config = provider.GetRequiredService<SampleConfig>();
				return new SampleSqlServerMigration(config.ConnectionString);
			});
		}
		public async Task<int> RunUtility(SqlServerMigration<SampleSqlServerMigration> svc) {
			await svc.ExecuteDeploymentScript(Options.Location ?? "Scripts");
			return 0;
		}
	}
}
