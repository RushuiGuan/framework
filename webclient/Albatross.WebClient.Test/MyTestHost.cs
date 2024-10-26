using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.WebClient.Test {
	public class MyTestHost : Hosting.Test.TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddTestClientService();
		}
	}
}