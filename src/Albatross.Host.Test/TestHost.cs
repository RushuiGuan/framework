using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Albatross.Host.Test{
	public class TestHost : IDisposable {
		protected IHost host;

		public TestHost() {
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			host = Microsoft.Extensions.Hosting.Host
				.CreateDefaultBuilder()
				.UseSerilog()
				.ConfigureServices((ctx,svc) => RegisterServices(ctx.Configuration, svc))
				.Build();

			Init(host.Services.GetRequiredService<IConfiguration>());
		}

		public IServiceProvider Provider => this.host.Services;
		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddTransient(provider => provider.CreateScope());
			services.AddTransient<TestScope>();
		}

		public TestScope Create() {
			return Provider.GetRequiredService<TestScope>();
		}

		public virtual void Init(IConfiguration configuration) { }

		public void Dispose() {
			host.Dispose();
			Log.CloseAndFlush();
		}
	}
}