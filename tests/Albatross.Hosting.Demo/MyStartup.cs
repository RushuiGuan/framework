using Albatross.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Hosting.Test {
	public class MyStartup : Startup {
		public override bool Grpc => false;
		public override bool Secured => false;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Spa => false;
		// public override bool Caching => true;

		public MyStartup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCaching();
			services.AddSingleton<ICacheManagement, IssuerCachedMgmt>();
			services.AddSingleton<ICacheManagement, SymbolCacheManagement>();
		}
	}
}