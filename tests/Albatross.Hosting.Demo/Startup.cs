using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Albatross.Hosting.Test {
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Hosting.Startup {
		public override bool Spa => true;
		public override bool Grpc => true;
		public override bool Swagger => true;
		public override bool WebApi => true;


		public Startup(IConfiguration configuration) : base(configuration) {
		}
	}
}