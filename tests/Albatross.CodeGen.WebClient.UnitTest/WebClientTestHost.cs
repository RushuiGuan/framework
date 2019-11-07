using Albatross.Host.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeGen.WebClient.UnitTest {
	public class WebClientTestHost: TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddDefaultCodeGen().AddCodeGen(this.GetType().Assembly);
			services.AddTransient<ConvertApiControllerToCSharpClass>();
		}
	}
}
