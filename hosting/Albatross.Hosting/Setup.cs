using Albatross.Config;
using Albatross.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public class Setup {
		protected IHostBuilder hostBuilder;
		protected IConfiguration configuration;
		string environment { get; init; }

		public Setup(string[] args) {
			environment = EnvironmentSetting.ASPNETCORE_ENVIRONMENT.Value;
			hostBuilder = Host.CreateDefaultBuilder(args).UseSerilog();
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true);
			if (!string.IsNullOrEmpty(environment)) { configBuilder.AddJsonFile($"appsettings.{environment}.json", true, true); }

			this.configuration = configBuilder.AddJsonFile("hostsettings.json", true, false)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			});
		}

		public virtual Setup RunAsService() {
			var setting = new ProgramSetting(configuration);
			switch (setting.ServiceManager) {
				case ProgramSetting.WindowsServiceManager:
					hostBuilder.UseWindowsService();
					break;
				case ProgramSetting.SystemDServiceManager:
					hostBuilder.UseSystemd();
					break;
				default:
					throw new ConfigurationException("Service configuration not set at Program__ServiceManager");
			}
			return this;
		}

		public virtual Setup ConfigureWebHost<Startup>() where Startup : Hosting.Startup {
			hostBuilder.ConfigureWebHostDefaults(webBuilder => {
				webBuilder.UseStartup<Startup>();
				webBuilder.PreferHostingUrls(true);
			});
			return this;
		}


		public virtual Setup ConfigureServiceHost<T>() where T : class, IHostedService {
			hostBuilder.ConfigureServices((hostContext, services) => {
				services.AddHostedService<T>();
			});
			return this;
		}

		public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			services.AddConfig<ProgramSetting>(true);
			services.TryAddSingleton<EnvironmentSetting>(EnvironmentSetting.ASPNETCORE_ENVIRONMENT);
			services.AddCustomLogger();
			services.TryAddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
		}

		public virtual async Task RunAsync(string[] args) {
			using Serilog.Core.Logger logger = new SetupSerilog().UseConfigFile(environment, null, args).Create();
			this.hostBuilder.ConfigureServices((context, services)=>this.ConfigureServices(services, context.Configuration));
			await this.hostBuilder.Build().RunAsync();
		}
	}
}