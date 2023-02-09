using Albatross.Caching.MemCache;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using Polly.Caching.Distributed;
using System.Threading.Tasks;

namespace Albatross.Caching.Test {
	public class MemCacheHost : TestHost {
		public const string HostType = "memcache";
		public ICacheManagementFactory CacheFactory { get; private set; } = null!;


		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCaching(configuration);
			services.AddCacheMgmt(typeof(SlidingTtlCacheMgmt).Assembly);
			services.AddMemCache();
		}

		public override async Task InitAsync(IConfiguration configuration, ILogger logger) {
			await base.InitAsync(configuration, logger);
			this.Provider.UseCache();
			this.CacheFactory = this.Provider.GetRequiredService<ICacheManagementFactory>();
		}
	}
}
