using Albatross.Config;
using Albatross.Config.Core;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
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
					ConfigureServices(services);
				});
			return hostBuilder;
		}
		

		public virtual async Task RunAsync(params string[] args) {
			using var setup = new SetupSerilog();
			setup.UseConfigFile("serilog.json");
			await Create(args).Build().RunAsync();
		}

		public virtual void ConfigureServices(IServiceCollection services) {
			services.AddConfig<ProgramSetting, GetProgramSetting>();
		}
	}
}