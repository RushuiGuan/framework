using Microsoft.Extensions.Configuration;

namespace Albatross.WebClient.TestApi {
	public class TestApiStartup : Albatross.Host.AspNetCore.Startup {
		public TestApiStartup(IConfiguration configuration) : base(configuration) {
		}
	}
}
