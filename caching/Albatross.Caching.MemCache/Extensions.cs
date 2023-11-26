using Albatross.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Caching;

namespace Albatross.Caching.MemCache {
	public static class Extensions {
		public static IServiceCollection AddMemCaching(this IServiceCollection services) {
			services.AddSingleton<ICacheKeyManagement, MemoryCacheKeyManagement>();
			services.TryAdd(ServiceDescriptor.Singleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>());
			services.AddSingleton<ICacheProviderAdapter, ObjectCacheProviderAdapter>();
			services.AddMemoryCache();
			return services;
		}
	}
}
