using Albatross.Config;
using Albatross.Config.Core;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Host.Worker {
	public class ServiceHost<T> where T : class, IHostedService {
		IHostBuilder Create(string[] args) {
			var configuration = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
				.UseSerilog();
			
			var setting = new GetProgramSetting(configuration).Get();

			switch (setting.ServiceManager) {
				case ProgramSetting.WindowsServiceManager:
					hostBuilder.UseWindowsService();
					break;
				case ProgramSetting.SystemDServiceManager:
					hostBuilder.UseSystemd();
					break;
			}

			hostBuilder.ConfigureAppConfiguration(builder => {
					builder.Sources.Clear();
					builder.AddConfiguration(configuration);
				}).ConfigureServices((hostContext, services) => {
					services.AddHostedService<T>();
					ConfigureServices(services);
				});
			return hostBuilder;
		}
		

		public virtual async Task RunAsync(params string[] args) {
			using var setup = new SetupSerilog();
			setup.UseConfigFile("serilog.json");
			//setup.UseConsoleAndFile(Serilog.Events.LogEventLevel.Information, null);
			await Create(args).Build().RunAsync();
		}

		public virtual void ConfigureServices(IServiceCollection services) {
			services.AddConfig<ProgramSetting, GetProgramSetting>();
		}
	}

	/*
	class Program {
		static Task Main(string[] args) {
			return new ServiceHost<MyHostedService>().RunAsync(args);
		}
	}

	public class MyHostedService : BackgroundService {
		private readonly ILogger<MyHostedService> logger;

		public MyHostedService(ILogger<MyHostedService> logger, ProgramSetting programSetting)  {
			this.logger = logger;
			logger.LogInformation("{class} instance created", nameof(MyHostedService));
			logger.LogInformation("Running {program}, environment: {environment}, service manager: {manager}", programSetting.App, programSetting.Environment, programSetting.ServiceManager);
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
			while (true) {
				await Task.Delay(1000);
				if (cancellationToken.IsCancellationRequested) {
					logger.LogInformation("cancel requested");
					break;
				}
			}
		}
	}
	*/
}