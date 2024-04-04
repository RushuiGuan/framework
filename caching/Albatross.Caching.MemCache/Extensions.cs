using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Caching.MemCache {
	public static class Extensions {
		public static IServiceCollection AddMemCaching(this IServiceCollection services) {
			services.TryAddSingleton<MemoryCacheKeyManagement>();
			services.TryAddSingleton<Polly.Caching.Memory.MemoryCacheProvider>();
			services.TryAddSingleton<ObjectCacheProviderAdapter>();
			services.AddSingleton<ICacheKeyManagement>(provider => provider.GetRequiredService<MemoryCacheKeyManagement>());
			services.AddSingleton<ICacheProviderAdapter>(provider => provider.GetRequiredService<ObjectCacheProviderAdapter>());
			services.AddMemoryCache();
			return services;
		}
		public static IServiceCollection AddMemCachingAsSecondary(this IServiceCollection services) {
			services.TryAddSingleton<MemoryCacheKeyManagement>();
			services.TryAddSingleton<Polly.Caching.Memory.MemoryCacheProvider>();
			services.TryAddSingleton<ObjectCacheProviderAdapter>();
			services.AddMemoryCache();
			return services;
		}
	}
}
