using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Invocation;

namespace Albatross.Messaging.Utility {
	public class MySetup : Setup {
		protected override string RootCommandDescription => "Albatross Messaging Utility";
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddSingleton<IMessageFactory, MessageFactory>();
			services.RegisterCommands();
		}
	}
}