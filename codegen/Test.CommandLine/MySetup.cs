using Albatross.CommandLine;
using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Invocation;
using Test.Proxy;

namespace Test.CommandLine {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddTestProxy();
			services.RegisterCommands();
		}
	}
}