using Albatross.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.Caching {
	// class name should not be renamed to Extensions due to backward compatibilies issue
	public static class Extension {
		public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration) {
			var registry = new PolicyRegistry();
			services.TryAdd(ServiceDescriptor.Singleton<IPolicyRegistry<string>>(registry));
			services.TryAdd(ServiceDescriptor.Singleton<IReadOnlyPolicyRegistry<string>>(registry));
			return services;
		}

		public static IServiceCollection AddCacheMgmt<T>(this IServiceCollection services) where T : class, ICacheManagement {
			services.TryAddSingleton<T>();
			services.AddSingleton<ICacheManagement>(provider => provider.GetRequiredService<T>());
			return services;
		}

		/// <summary>
		/// Register all ICacheManagement implementation of an assembly using the singleton scope.  The search of the concrete class is performed using the generic type ICacheManagement<>
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IServiceCollection AddCacheMgmt(this IServiceCollection services, Assembly assembly) {
			if (assembly != null) {
				Type genericDefinition = typeof(ICacheManagement<,>);
				foreach (Type type in assembly.GetConcreteClasses()) {
					if (type.TryGetClosedGenericType(genericDefinition, out Type? _)) {
						services.TryAddSingleton(type);
						services.AddSingleton<ICacheManagement>(provider => (ICacheManagement)provider.GetRequiredService(type));
					}
				}
			}
			return services;
		}
	}
}
