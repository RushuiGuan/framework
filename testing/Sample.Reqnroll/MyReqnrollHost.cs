using Albatross.ReqnrollPlugin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.BoDi;
using System.Reflection;

namespace Sample.Reqnroll {
	[ReqnrollHost]
	public class MyReqnrollHost : ReqnrollHost {
		public MyReqnrollHost(Assembly testAssembly, IObjectContainer rootBoDiContainer) : base(testAssembly, rootBoDiContainer) {
		}
		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			base.ConfigureServices(services, configuration);
		}
	}
}
