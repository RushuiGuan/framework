using Microsoft.Extensions.Configuration;

namespace Sample.Hosting.WebApi {
	public class Startup : Albatross.Hosting.Startup {
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;
		public override bool Spa => true;
		public override bool Caching => false;
		public Startup(IConfiguration configuration) : base(configuration) { }
	}
}
