using Microsoft.Extensions.Configuration;

namespace Test.WebApi {
	public class MyStartup : Albatross.Hosting.Startup {
		public MyStartup(IConfiguration configuration) : base(configuration) {
		}
	}
}