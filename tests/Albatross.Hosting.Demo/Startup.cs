using Albatross.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Hosting.Test {
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Hosting.Startup {
		public override bool Spa => true;
		public override bool Grpc => true;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;
		public override bool Caching => true;

		public Startup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCaching();
			services.AddSingleton<ICacheManagement, IssuerCachedMgmt>();
			services.AddSingleton<ICacheManagement, SymbolCacheManagement>();
		}
	}
}