using Albatross.Config.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Host.PowerShell {
	public class Session {
		IServiceCollection services = new ServiceCollection();
        ServiceProvider provider;


		public virtual void RegisterServices(IServiceCollection services) {		}


		public Session Start() {
            services.AddCustomConfig(this.GetType().Assembly, true);
			RegisterServices(services);
            provider = services.BuildServiceProvider();
			return this;
		}
        public T GetRequiredService<T>() => provider.GetRequiredService<T>();
		public string AccessToken { get; set; }
	}
}
