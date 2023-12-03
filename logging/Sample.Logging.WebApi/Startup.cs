using Microsoft.Extensions.Configuration;

namespace Sample.Logging.WebApi {
	public class Startup : Albatross.Hosting.Startup {
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;
		public override bool Spa => false;
		public override bool Caching => false;

		public Startup(IConfiguration configuration) : base(configuration) { }
	}
}
