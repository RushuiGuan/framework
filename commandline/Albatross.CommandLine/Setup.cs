using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Hosting;
using System.CommandLine.Builder;
using System.Threading.Tasks;
using System.Linq;

namespace Albatross.CommandLine {
	public class Setup {
		public Setup() {
			Console.WriteLine("Setting up");
			this.RootCommand = CreateRootCommand();
			this.CommandBuilder = new CommandLineBuilder(this.RootCommand);
			this.CommandBuilder.AddMiddleware(AddLoggingMiddleware);
			this.CommandBuilder.AddMiddleware(SetCommandHanlerMiddleware);
			this.CommandBuilder.UseHost(args => Host.CreateDefaultBuilder(), hostBuilder => {
				var environment = EnvironmentSetting.DOTNET_ENVIRONMENT;
				var logger = new SetupSerilog().Configure(ConfigureLogging).Create();
				hostBuilder.UseSerilog(logger);
				logger.Information("Building Host");
				var configBuilder = new ConfigurationBuilder()
					.SetBasePath(System.IO.Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", true, false);
				if (!string.IsNullOrEmpty(environment.Value)) {
					configBuilder.AddJsonFile($"appsettings.{environment}.json", true, false);
				}
				logger.Information("Creating configuration");
				var configuration = configBuilder.AddEnvironmentVariables().Build();
				hostBuilder.ConfigureAppConfiguration(builder => {
					builder.Sources.Clear();
					builder.AddConfiguration(configuration);
				}).ConfigureServices((context, svc) => RegisterServices(context.Configuration, environment, svc));
			});
			this.CommandBuilder.UseDefaults();
			Console.WriteLine("Setup complete");
		}

		private Task AddLoggingMiddleware(InvocationContext context, Func<InvocationContext, Task> next) {
			Console.WriteLine("Adding logging middleware");
			var logOption = this.RootCommand.Options.OfType<Option<LogEventLevel?>>().First();
			var result = context.ParseResult.GetValueForOption(logOption);
			if (result != null) {
				SetupSerilog.SwitchConsoleLoggingLevel(result.Value);
			}
			return next(context);
		}

		private Task SetCommandHanlerMiddleware(InvocationContext context, Func<InvocationContext, Task> next) {
			var cmd = context.ParseResult.CommandResult.Command;
			Console.WriteLine($"Set command handler for {cmd.Name}");
			cmd.Handler = new CommandHandlerFactory(cmd);
			return next(context);
		}


		public virtual RootCommand CreateRootCommand() {
			var cmd = new RootCommand();
			cmd.AddGlobalOption(new Option<LogEventLevel?>("--log", () => LogEventLevel.Error) { IsHidden = true });
			cmd.AddGlobalOption(new Option<bool>("--clipboard") { IsHidden = true });
			cmd.AddGlobalOption(new Option<bool>("--benchmark") { IsHidden = true });
			cmd.AddGlobalOption(new Option<bool>("--show-stack") { IsHidden = true });
			return cmd;
		}
		public RootCommand RootCommand { get; }
		public CommandLineBuilder CommandBuilder { get; }

		public Setup AddCommand<TCommand>() where TCommand : Command, new() {
			var cmd = new TCommand();
			this.RootCommand.Add(cmd);
			return this;
		}

		public virtual void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			Serilog.Log.Information("Registering services");
			services.AddConfig<ProgramSetting>();
			services.AddSingleton(envSetting);
			services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
			services.AddSingleton<IHostEnvironment, MyHostEnvironment>();
			services.AddOptions<GlobalOptions>().BindCommandLine();
			Serilog.Log.Information("Registering services - done");
		}

		protected virtual void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConsole(cfg, null);
		}
	}
}
