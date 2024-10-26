using Albatross.Text;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Albatross.CodeGen.Python {
	public static class Extensions {
		public static CodeGeneratorScope BeginPythonScope(this TextWriter writer) {
			return new CodeGeneratorScope(writer, args => args.Append(":"), args => { });
		}
		public static IServiceCollection AddPythonCodeGen(this IServiceCollection services) {
			services.AddCodeGen(typeof(Extensions).Assembly);
			return services;
		}
	}
}