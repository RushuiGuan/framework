using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;

namespace Albatross.Messaging.Utility {
	public class MySetup : Setup {
		protected override string RootCommandDescription => "Albatross Messaging Utility";
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddSingleton<IMessageFactory, MessageFactory>();
			services.AddOptions<MessagingGlobalOptions>().BindCommandLine();
			services.RegisterCommands();
		}
		public override RootCommand CreateRootCommand() {
			var cmd = base.CreateRootCommand();
			var appOption = new Option<string>("--application", "The application name") {
				IsRequired = true,
			};
			appOption.AddAlias("-a");
			cmd.AddGlobalOption(appOption);

			var folderOption = new Option<string>("--event-source-folder", "The folder where the event source files are located");
			folderOption.AddAlias("-f");
			cmd.AddGlobalOption(folderOption);
			return cmd;
		}
	}
}