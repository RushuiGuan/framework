using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Albatross.Hosting.PowerShell {
	public class Session {
		IHost host;
		
		public Session Start() {
			host = Microsoft.Extensions.Hosting.Host
				.CreateDefaultBuilder()
				.ConfigureServices((context, services) => RegisterServices(context.Configuration, services))
				.Build();
			return this;
		}

		public virtual void RegisterServices(IConfiguration configuration, IServiceCollection services) { }
		public T GetRequiredService<T>() => this.host.Services.GetRequiredService<T>();
		public string AccessToken { get; set; }
	}
}
