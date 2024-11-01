using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Testing.DependencyInjection {
	public class TestHostBuilder {
		IHostBuilder hostBuilder;
		Func<IConfiguration> getConfig = () => new ConfigurationBuilder().Build();
		Action<IConfiguration, IServiceCollection> registerServices = (config, services) => { };
		public TestHostBuilder() {
			hostBuilder = Host.CreateDefaultBuilder();
		}
		public TestHostBuilder WithLogging() {
			this.registerServices += (config, services) => {
				services.AddLogging(builder => builder.AddConsole());
				services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));
			};
			return this;
		}
		public TestHostBuilder WithAppSettingsConfiguration(string environment) {
			this.getConfig = () => new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", true, false)
				.AddJsonFile($"appsettings.{environment}.json", true, false)
				.Build();
			return this;
		}
		public TestHostBuilder WithConfiguration(IConfiguration configuration) {
			this.getConfig = () => configuration;
			return this;
		}
		public TestHostBuilder RegisterServices(Action<IConfiguration, IServiceCollection> register) {
			this.registerServices += register;
			return this;
		}
		public IHost Build() {
			return hostBuilder.ConfigureAppConfiguration(builder => {
				builder.Sources.Clear();
				builder.AddConfiguration(getConfig());
			}).ConfigureServices((ctx, svc) => this.registerServices(ctx.Configuration, svc))
			.Build();
		}
	}
}