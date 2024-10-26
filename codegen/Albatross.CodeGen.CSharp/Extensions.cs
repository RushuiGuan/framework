using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.CSharp {
	public static class Extensions {
		public static IServiceCollection AddCSharpCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extensions).Assembly);
			return services;
		}
	}
}