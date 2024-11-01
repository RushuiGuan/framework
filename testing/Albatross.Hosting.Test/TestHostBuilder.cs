using Albatross.Config;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using Serilog;
using System;
using System.IO;

namespace Albatross.DependencyInjection.Testing {
	public class TestHostBuilder {
		IHostBuilder hostBuilder;
		Func<IConfiguration> getConfig;
		Action<IConfiguration, IServiceCollection> registerServices = (config, services)=> { };
		public TestHostBuilder() {
			hostBuilder = Host.CreateDefaultBuilder();
		}
		public TestHostBuilder WithLogging() {
			new SetupSerilog().UseConsole(Serilog.Events.LogEventLevel.Information);
			hostBuilder.UseSerilog();
			return this;
		}
		public TestHostBuilder WithAppSettingsConfig(string environment) {
			this.getConfig = () => new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", true, false)
				.AddJsonFile($"appsettings.{environment}.json", true, false)
				.Build();
			return this;
		}
		public TestHostBuilder WithCustomConfiguration(IConfiguration configuration){
			this.getConfig = () => configuration;
			return this;
		}
		public TestHostBuilder RegisterServices(Action<IConfiguration, IServiceCollection> register) {
			this.registerServices += register;
			return this;
		}
		public IHost Build() {
			host = hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(configuration);
			}).ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
			.Build();
		}

		public TestHost() {
			var hostBuilder = Host.CreateDefaultBuilder().UseSerilog();
			var configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", true, false)
				.AddJsonFile($"appsettings.{Environment}.json", true, false)
				.AddEnvironmentVariables()
				.Build();

			
		}


		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddSingleton(new EnvironmentSetting(Environment));
		}

		public IServiceScope Create() {
			return Provider.CreateAsyncScope();
		}

		public void Dispose() {
			host.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}