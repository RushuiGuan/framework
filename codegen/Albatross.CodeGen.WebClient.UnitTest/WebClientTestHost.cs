using Albatross.CodeGen.CSharp;
using Albatross.CodeGen.TypeScript;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class WebClientTestHost: TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCSharpCodeGen().AddTypeScriptCodeGen()
				.AddCodeGen(this.GetType().Assembly);
			services.AddTransient<ConvertApiControllerToCSharpClass>();
		}
	}
}
