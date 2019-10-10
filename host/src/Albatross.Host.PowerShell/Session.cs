using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Host.PowerShell {
	public class Session {
		IServiceCollection services = new ServiceCollection();
        ServiceProvider provider;


		public virtual void RegisterServices(IServiceCollection services) {		}


		public Session Start() {
			var setup = new SetupConfig(this.GetType().GetAssemblyLocation());
			setup.RegisterServices(services);
			RegisterServices(services);
            provider = services.BuildServiceProvider();
			return this;
		}
        public T GetRequiredService<T>() => provider.GetRequiredService<T>();
		public string AccessToken { get; set; }
	}
}
