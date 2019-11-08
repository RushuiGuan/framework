using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Albatross.Logging;

namespace Albatross.Host.Test {
	public class TestHost : IDisposable {
		protected IHost host;
		private SetupSerilog setupSerilog;
		public IServiceProvider Provider => this.host.Services;

		public TestHost() {
			setupSerilog = new SetupSerilog();
			host = Microsoft.Extensions.Hosting.Host
				.CreateDefaultBuilder()
				.UseSerilog()
				.ConfigureServices((ctx, svc) => RegisterServices(ctx.Configuration, svc))
				.Build();

			Init(host.Services.GetRequiredService<IConfiguration>());
		}

		public virtual void Init(IConfiguration configuration) { }

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			services.AddTransient(provider => provider.CreateScope());
			services.AddTransient<TestScope>();
		}

		public TestScope Create() {
			return Provider.GetRequiredService<TestScope>();
		}

		public void Dispose() {
			host.Dispose();
			setupSerilog.Dispose();
		}
	}
}