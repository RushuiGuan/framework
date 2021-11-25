using Albatross.CodeGen.Core;
using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Albatross.CodeGen {
	public static class ServiceExtension {
		public static IServiceCollection AddCodeGen(this IServiceCollection services, Assembly assembly) {
			foreach (var type in assembly.GetTypes()) {
				TryAddConverter(services, type);
			}
			return services;
		}

		public static IServiceCollection AddDefaultCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(ServiceExtension).Assembly);
			return services;
		}

		public static bool TryAddConverter(this IServiceCollection services, Type converterType) {
			if (converterType.TryGetClosedGenericType(typeof(IConvertObject<>), out Type genericInterfaceType)) {
				services.AddTransient(converterType);
				services.AddTransient(genericInterfaceType, converterType);
				if (converterType.TryGetClosedGenericType(typeof(IConvertObject<,>), out genericInterfaceType)) {
					services.AddTransient(genericInterfaceType, converterType);
				}
				return true;
			} else {
				return false;
			}
		}
	}
}

