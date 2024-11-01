using Microsoft.Extensions.Configuration;

namespace Test.WebApi {
	public class MyStartup : Albatross.DependencyInjection.Startup {
		public MyStartup(IConfiguration configuration) : base(configuration) {
		}
	}
}