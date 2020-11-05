using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Albatross.Hosting.PowerShell {
	public class Session {
		IHost host;
		
		public virtual Session Start() {
			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			string basePath = this.GetType().Assembly.Location;
			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json", false, true)
				.AddEnvironmentVariables()
				.Build();

			hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			});

			var setupSerilog = new SetupSerilog();
			setupSerilog.UseConfigFile("serilog.json", basePath);
			host = hostBuilder.ConfigureServices(this.ConfigureServices).Build();
			return this;
		}

		public virtual void ConfigureServices(IServiceCollection services) { }
		public T GetRequiredService<T>() => this.host.Services.GetRequiredService<T>();
		public string AccessToken { get; set; }
	}
}
