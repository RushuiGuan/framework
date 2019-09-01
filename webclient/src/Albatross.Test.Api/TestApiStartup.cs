using Microsoft.Extensions.Configuration;

namespace Albatross.Test.Api {
	public class TestApiStartup : Albatross.Host.AspNetCore.Startup {
		public TestApiStartup(IConfiguration configuration) : base(configuration) {
		}
	}
}
