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
using System.IO;

namespace Albatross.Hosting.CommandLine {
	public class StartUp {
		public StartUp() { }
		public RootCommand RootCommand { get; } = new RootCommand();
		List<Action<IHostBuilder>> actions = new List<Action<IHostBuilder>>();

		public StartUp AddCommand<TCommand, THandler>() where TCommand : Command, new() where THandler : ICommandHandler {
			RootCommand.AddCommand(new TCommand());
			actions.Add(builder => builder.UseCommandHandler<TCommand, THandler>());
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

		public virtual void BuildHost(IHostBuilder hostBuilder) {
			var env = EnvironmentSetting.DOTNET_ENVIRONMENT;
			var setup = new SetupSerilog().Configure(ConfigureLogging).Create();
			hostBuilder.UseSerilog();
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", true, false);
			if (!string.IsNullOrEmpty(env.Value)) { configBuilder.AddJsonFile($"appsettings.{env.Value}.json", true, false); }
			var configuration = configBuilder.AddEnvironmentVariables().Build();

			hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, env, svc));
			foreach (var action in this.actions) {
				action(hostBuilder);
			}
		}
	}
}
