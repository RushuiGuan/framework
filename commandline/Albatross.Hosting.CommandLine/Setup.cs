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
using System.Collections.Generic;
using System.CommandLine.Hosting;
using System.CommandLine.Builder;

namespace Albatross.Hosting.CommandLine {
	public class Setup {
		public Setup() {
			this.CommandBuilder = new CommandLineBuilder(RootCommand);
			this.CommandBuilder.UseHost(args => Host.CreateDefaultBuilder(), hostBuilder => {
				var environment = EnvironmentSetting.DOTNET_ENVIRONMENT;
				var logger = new SetupSerilog().Configure(ConfigureLogging).Create();
				hostBuilder.UseSerilog(logger);
				var configBuilder = new ConfigurationBuilder()
					.SetBasePath(System.IO.Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", true, false);
				if (!string.IsNullOrEmpty(environment.Value)) { configBuilder.AddJsonFile($"appsettings.{environment}.json", true, false); }

				var configuration = configBuilder.AddEnvironmentVariables().Build();

				hostBuilder.ConfigureAppConfiguration(builder => {
					builder.Sources.Clear();
					builder.AddConfiguration(configuration);
				}).ConfigureServices((context, svc) => RegisterServices(context.Configuration, environment, svc));
				RootCommand.Handler = new RootCommandHandler();
				foreach (var action in this.commandRegistrations) {
					action(hostBuilder);
				}
			});
		}
		protected virtual RootCommand RootCommand { get; } = new RootCommand { Name = "root",
			 
		};
		public CommandLineBuilder CommandBuilder { get; }
		List<Action<IHostBuilder>> commandRegistrations = new List<Action<IHostBuilder>>();

		public Setup AddCommand<TCommand, THandler>() where TCommand : Command, new() where THandler : ICommandHandler {
			RootCommand.AddCommand(new TCommand());
			commandRegistrations.Add(hostBuilder => hostBuilder.UseCommandHandler<TCommand, THandler>());
			return this;
		}

		public virtual void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			services.AddConfig<ProgramSetting>();
			services.AddSingleton(envSetting);
			services.AddSingleton(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
			services.AddSingleton<IHostEnvironment, MyHostEnvironment>();
			services.AddOptions<GlobalOptions>().BindCommandLine();
		}

		protected virtual void ConfigureLogging(LoggerConfiguration cfg) {
			SetupSerilog.UseConsole(cfg, LogEventLevel.Debug);
		}
	}
}
