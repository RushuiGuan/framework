using Albatross.CommandLine;
using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Proxy;
using System.CommandLine.Invocation;

namespace Sample.Utility {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.RegisterCommands();
			services.AddSampleProjectProxy();
		}
	}
}