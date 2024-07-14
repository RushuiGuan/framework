using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript {
	public static class Extensions {
		public static IServiceCollection AddTypeScriptCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extensions).Assembly);
			services.AddScoped<ITypeConverterFactory, TypeConverterFactory>();
			services.AddTypeConverters(typeof(TypeConverterFactory).Assembly);
			return services;
		}


		public static IServiceCollection AddTypeConverters(this IServiceCollection services, Assembly assembly) {
			foreach (var type in assembly.GetTypes()) {
				if(type.IsConcreteType() && type.IsDerived<ITypeConverter>()) {
					services.TryAddEnumerable(ServiceDescriptor.Scoped(typeof(ITypeConverter), type));
				}
			}
			return services;
		}
	}
}
