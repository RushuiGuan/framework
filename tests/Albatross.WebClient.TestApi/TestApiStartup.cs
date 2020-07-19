using Albatross.Config;
using Albatross.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Albatross.WebClient.TestApi {
	public class TestApiStartup : Startup {
		public TestApiStartup(IConfiguration configuration) : base(configuration) {
		}


		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddScoped<DatabaseSetting>(provider => this.Configuration.GetSection(DatabaseSetting.Key).Get<DatabaseSetting>());

		}
	}
}
