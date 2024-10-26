using Albatross.SpecFlowPlugin;
using BoDi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Sample.Spec {

	[SpecFlowHost]
	public class MySpecFlowHost : SpecFlowHost {
		public MySpecFlowHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			base.ConfigureServices(services, configuration);
		}
	}
}