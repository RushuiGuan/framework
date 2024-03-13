using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.EFCore.AutoCacheEviction {
	public static class Extensions {
		public static IServiceCollection AddAutoCacheEviction(this IServiceCollection services) {
			services.TryAddEnumerable(ServiceDescriptor.Scoped<IDbSessionEventHandler, AutoCacheEvictionDbEventHander>());
			return services;
		}

		public static bool TryGetOriginalValue(this IEnumerable<PropertyEntry> changeProperties, string property, Func<object?, string> format, out string originalValue) {
			var propertyEntry = changeProperties.FirstOrDefault(x => x.Metadata.Name == property);
			if(propertyEntry != null) {
				originalValue = format(propertyEntry.OriginalValue);
				return true;
			} else {
				originalValue = string.Empty;
				return false;
			}
		}
	}
}
