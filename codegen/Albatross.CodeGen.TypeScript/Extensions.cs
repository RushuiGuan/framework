using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.TypeScript {
	public static class Extensions {
		public static IServiceCollection AddTypeScriptCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extensions).Assembly);
			return services;
		}
	}
}
