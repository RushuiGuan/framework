using Albatross.CodeGen.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen {
	public static class ServiceExtension {
		public static IServiceCollection AddCodeGen(this IServiceCollection services, Assembly assembly) {
			foreach (var type in assembly.GetTypes()) {
				TryAddConverter(services, type);
				TryAddCodeGenerator(services, type);
			}
			return services;
		}

		public static IServiceCollection AddDefaultCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(ServiceExtension).Assembly);
			return services;
		}

		public static bool TryAddCodeGenerator(this IServiceCollection services, Type codeGenType) {
			if (codeGenType.TryGetGenericInterfaceType(typeof(ICodeGenerator<>), out Type genericInterfaceType)) {
				// register any ICodeGenerator
				services.AddTransient(codeGenType);
				services.AddTransient(genericInterfaceType, codeGenType);
				return true;
			} else {
				return false;
			}
		}
		public static bool TryAddConverter(this IServiceCollection services, Type converterType) {
			if (converterType.TryGetGenericInterfaceType(typeof(IConvertObject<>), out Type genericInterfaceType)) {
				services.AddTransient(converterType);
				//register any close implementation of IConvertObject<>
				services.AddTransient(genericInterfaceType, converterType);
				if (converterType.TryGetGenericInterfaceType(typeof(IConvertObject<,>), out genericInterfaceType)) {
					services.AddTransient(genericInterfaceType, converterType);
				}
				return true;
			} else {
				return false;
			}
		}
	}
}

