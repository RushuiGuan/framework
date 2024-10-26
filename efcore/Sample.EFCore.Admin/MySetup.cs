using Albatross.CommandLine;
using Albatross.Config;
using Albatross.EFCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Invocation;

namespace Sample.EFCore.Admin {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddSample()
				.AddScoped<SqlServerMigration<SampleSqlServerMigration>>()
				.AddScoped<SampleSqlServerMigration>(provider => {
					var config = provider.GetRequiredService<SampleConfig>();
					return new SampleSqlServerMigration(config.ConnectionString);
				}).AddScoped<ISqlBatchExecution, SqlBatchExecution>();
			services.RegisterCommands();
		}
	}
}