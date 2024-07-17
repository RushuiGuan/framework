using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Conversions;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript {
	public static class Extensions {
		public static IServiceCollection AddTypeScriptCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extensions).Assembly);
			services.AddScoped<IConvertObject<ITypeSymbol, ITypeExpression>, ConvertType>();
			services.AddTypeConverters(typeof(ConvertType).Assembly);
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

		public static bool IsPromise(this ITypeExpression type) {
			return type is GenericTypeExpression generic && generic.Identifier == Defined.Identifiers.Promise;
		}

		public static ITypeExpression ToPromise(this ITypeExpression type) {
			if (!type.IsPromise()) {
				type = new GenericTypeExpression(Defined.Identifiers.Promise) {
					Arguments = new ListOfSyntaxNodes<ITypeExpression>(type),
				};
			}
			return type;
		}
	}
}
