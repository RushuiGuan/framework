using Albatross.CommandLine;
using Albatross.Config;
using AzureDevOpsProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Albatross.DevTools {
	public class MySetup : Setup {
		protected override string RootCommandDescription => "Albatross Dev Tools";
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddAzureDevOpsProxy();
			services.RegisterCommands();
		}

		public override ICommandHandler CreateGlobalCommandHandler(Command command) {
			return base.CreateGlobalCommandHandler(command);
		}
	}
}
