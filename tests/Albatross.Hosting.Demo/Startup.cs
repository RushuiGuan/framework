using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System;

namespace Albatross.Hosting.Test {
	public static class CachingPolicy {
		public const string FiveMinuteTTL = "FiveMinuteTTL";
		public const string TenMinuteTTL = "TenMinuteTTL";
	}
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Hosting.Startup {
		public override bool Spa => true;
		public override bool Grpc => true;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;

		public Startup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddMemoryCache();
			services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
			services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(
				provider => {
					var registry = new PolicyRegistry();
					registry.Add(CachingPolicy.FiveMinuteTTL, Policy.CacheAsync<int[]>(provider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<int[]>(), TimeSpan.FromMinutes(5)));
					registry.Add(CachingPolicy.TenMinuteTTL, Policy.CacheAsync<string[]>(provider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<string[]>(), TimeSpan.FromMinutes(10)));
					return registry;
				}
			);
			services.AddSingleton<IAsyncPolicy<int[]>>(provider => provider.GetRequiredService<IReadOnlyPolicyRegistry<string>>().Get<IAsyncPolicy<int[]>>(CachingPolicy.FiveMinuteTTL));
			services.AddSingleton<IAsyncPolicy<string[]>>(provider => provider.GetRequiredService<IReadOnlyPolicyRegistry<string>>().Get<IAsyncPolicy<string[]>>(CachingPolicy.TenMinuteTTL));
		}
	}
}