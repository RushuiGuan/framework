using Albatross.Config;
using Albatross.Hosting.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.CommandLine {
	public class MySetup : Setup{
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.RegisterCommands();
		}
	}
}
