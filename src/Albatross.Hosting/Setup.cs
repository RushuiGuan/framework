using Albatross.Config;
using Albatross.Config.Core;
using Albatross.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public class Setup {
		IHostBuilder hostBuilder;
		protected IConfiguration configuration;

		public Setup(string[] args) {
			hostBuilder = Host.CreateDefaultBuilder(args).UseSerilog();
			var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			configuration = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{env}.json", true, true)
				.AddJsonFile("hostsettings.json", true, false)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			});
		}

		public Setup RunAsService() {
			var setting = new GetProgramSetting(configuration).Get();
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

		public Setup ConfigureWebHost<Startup>() where Startup : Hosting.Startup{
			hostBuilder.ConfigureWebHostDefaults(webBuilder => {
				webBuilder.UseStartup<Startup>();
				webBuilder.PreferHostingUrls(true);
			});
			return this;
		}


		public Setup ConfigureServiceHost<T>() where T:class, IHostedService {
			hostBuilder.ConfigureServices((hostContext, services) => {
				services.AddHostedService<T>();
			});
			return this;
		}

		public virtual void ConfigureServices(IServiceCollection services) {
			services.AddConfig<ProgramSetting, GetProgramSetting>();
		}


		public async Task RunAsync() {
			this.hostBuilder.ConfigureServices(this.ConfigureServices);
			using var logger = new SetupSerilog().UseConfigFile("serilog.json").Create();
			await this.hostBuilder.Build().RunAsync();
		}
	}
}
