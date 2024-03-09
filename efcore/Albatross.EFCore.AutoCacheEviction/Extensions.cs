using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Albatross.EFCore.AutoCacheEviction {
	public static class Extensions {
		public static IServiceCollection AddAutoCacheEviction(this IServiceCollection services) {
			services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, AutoCacheEvictionDbEventHander>());
			return services;
		}
	}
}
