using Albatross.Caching.Redis;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.Caching.Test {
	public class RedisCacheHost : TestHost {
		public const string HostType = "redis";


		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCaching(configuration);
			services.AddCacheMgmt(typeof(SlidingTtlCacheMgmt).Assembly);
			services.AddRedisCaching(configuration);
		}

		public override async Task InitAsync(IConfiguration configuration, ILogger logger) {
			await base.InitAsync(configuration, logger);
			this.Provider.UseCache();
			this.Provider.UseRedisCaching();
		}
	}
}
