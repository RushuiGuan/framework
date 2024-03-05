using Albatross.Caching.MemCache;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Caching.Test {
	public class MemCacheHost : TestHost {
		public const string HostType = "memcache";

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCaching(configuration);
			services.AddBuiltInCache();
			services.AddMemCaching();
		}
	}
}
