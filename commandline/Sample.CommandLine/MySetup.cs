using Albatross.CommandLine;
using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

namespace Sample.CommandLine {
	public class MySetup : Setup {
		protected override string RootCommandDescription => "This a sample command";
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddShortenLoggerName(false, "Albatross", "Sample");
			services.RegisterCommands();
		}
	}

	public static class SetupExtensions {
		public static Setup AddMyCommands(this Setup setup) {
			setup.AddCommand<MySuperCommand>();
			return setup;
		}
	}

	public class MySub1Command : Command {
		public MySub1Command() : base("sub1", "my sub1 command") {
			this.Handler = CommandHandler.Create(() => {
				System.Console.WriteLine("sub 1 command");
			});
		}
	}
	public class MySub2Command : Command {
		public MySub2Command() : base("sub2", "my sub2 command") {
			this.Handler = CommandHandler.Create(() => {
				System.Console.WriteLine("sub 2 command");
			});
		}
	}

	public class MySuperCommand : Command {
		public MySuperCommand() : base("super", "a test parent command") {
			this.Handler = new HelpCommandHandler();
			this.AddCommand(new MySub1Command());
			this.AddCommand(new MySub2Command());
		}
	}
}