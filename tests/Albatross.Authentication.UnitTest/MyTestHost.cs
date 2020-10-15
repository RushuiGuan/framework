using Albatross.Authentication.Server;
using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication.UnitTest {
	public class MyTestHost : TestHost{
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddWindowsPrincipalProvider();
			services.AddWindowsAuthenticationServerTest();
		}
	}
}