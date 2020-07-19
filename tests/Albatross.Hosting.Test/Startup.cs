using Microsoft.Extensions.Configuration;

namespace Albatross.Hosting.Test {
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Hosting.Startup {
		public Startup(IConfiguration configuration) : base(configuration) {
		}
	}
}