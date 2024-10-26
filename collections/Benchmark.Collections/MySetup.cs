using Albatross.CommandLine;
using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Invocation;

namespace Benchmark.Collections {
	public class MySetup : Setup {
		protected override string RootCommandDescription => "Albatross Collections Benchmark Utility";
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.RegisterCommands();
		}
	}
}