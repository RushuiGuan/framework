using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.EFCore.AutoCacheEviction {
	public static class Extensions {
		public static IServiceCollection AddAutoCacheEviction(this IServiceCollection services) {
			//services.TryAddScoped<AutoCacheEvictionDbSessionEventHander>();
			services.TryAddScoped<IDbChangeEventHandler, AutoCacheEvictionDbSessionEventHander>();
			return services;
		}
	}
}
