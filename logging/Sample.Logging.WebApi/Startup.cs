using Albatross.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Logging.WebApi {
	public class Startup : Albatross.Hosting.Startup {
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;
		public override bool Spa => false;
		public override bool Caching => false;

		public Startup(IConfiguration configuration) : base(configuration) { }
		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddShortenLoggerName(true, "Microsoft", "System");
		}
	}
}
