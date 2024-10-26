using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.EFCore.AutoCacheEviction {
	public static class Extensions {
		public static IServiceCollection AddAutoCacheEviction(this IServiceCollection services) {
			services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, AutoCacheEvictionDbEventHander>());
			return services;
		}

		public static bool TryGetOriginalValue<T>(this IEnumerable<PropertyEntry> changeProperties, string property, out T? originalValue) {
			var propertyEntry = changeProperties.FirstOrDefault(x => x.Metadata.Name == property);
			if (propertyEntry != null) {
				if (propertyEntry.OriginalValue == null) {
					originalValue = default(T);
				} else {
					originalValue = (T)propertyEntry.OriginalValue;
				}
				return true;
			} else {
				originalValue = default(T);
				return false;
			}
		}
	}
}