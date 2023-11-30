using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.Caching.MemCache {
	public static class Extensions {
		public static IServiceCollection AddMemCaching(this IServiceCollection services) {
			services.AddSingleton<MemoryCacheKeyManagement>();
			services.AddSingleton<ICacheKeyManagement>(provider => provider.GetRequiredService<MemoryCacheKeyManagement>());
			services.TryAddSingleton<Polly.Caching.Memory.MemoryCacheProvider>();
			services.AddSingleton<ObjectCacheProviderAdapter>();
			services.AddSingleton<ICacheProviderAdapter>(provider => provider.GetRequiredService<ObjectCacheProviderAdapter>());
			services.AddMemoryCache();
			return services;
		}
		public static IServiceCollection AddMemCachingAsSecondary(this IServiceCollection services) {
			services.AddSingleton<MemoryCacheKeyManagement>();
			services.AddSingleton<Polly.Caching.Memory.MemoryCacheProvider>();
			services.AddSingleton<ObjectCacheProviderAdapter>();
			services.AddMemoryCache();
			return services;
		}
	}
}
