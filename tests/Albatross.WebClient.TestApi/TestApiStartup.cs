using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.WebClient.TestApi {
	public class TestApiStartup : Albatross.Host.AspNetCore.Startup {
		public TestApiStartup(IConfiguration configuration) : base(configuration) {
		}


		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddScoped<DatabaseSetting>(provider => this.Configuration.GetSection(DatabaseSetting.Key).Get<DatabaseSetting>());

		}
	}
}
