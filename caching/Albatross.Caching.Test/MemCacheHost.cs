using Albatross.Caching.MemCache;
using Albatross.Caching.Test.CacheMgmt;
using Albatross.Caching.TestApi;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.Caching.Test {
	public class MemCacheHost : TestHost {
		public const string HostType = "memcache";

		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCaching(configuration);
			services.AddCacheMgmt(typeof(SlidingTtlCacheMgmt).Assembly);
			services.AddCacheMgmt(typeof(MultiTierKey).Assembly);
			services.AddMemCaching();
		}
	}
}
