using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.CommandLine.Invocation;

namespace Sample.CommandLine {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
		}
		protected override void ConfigureLogging(LoggerConfiguration cfg) {
			// base.ConfigureLogging(cfg);
			// configure serilog your own way here
			cfg.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
				.WriteTo.Console(outputTemplate: SetupSerilog.DefaultOutputTemplate)
				.Enrich.FromLogContext();
		}
	}
}