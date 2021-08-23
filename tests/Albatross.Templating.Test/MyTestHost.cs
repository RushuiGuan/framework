using Albatross.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Templating.Test {
	public class MyTestHost : Albatross.Hosting.Test.TestHost{
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddWindowsPrincipalProvider();
		}
	}
}
