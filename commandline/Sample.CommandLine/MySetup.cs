using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Invocation;

namespace Sample.CommandLine {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddShortenLoggerName(false, "Albatross", "Sample");
			services.RegisterCommands();
		}
	}
}