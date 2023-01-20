using Albatross.Caching.Redis;
using Albatross.Config;
using Albatross.Hosting;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Albatross.Caching.TestHost {
	public class MyStartup : Startup {
		public override bool Grpc => false;
		public override bool Spa => false;
		public override bool Secured => true;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Caching => true;

		public MyStartup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCacheMgmt(this.GetType().Assembly);
			services.AddRedisCaching(this.Configuration);
			services.AddSignalR();
			services.AddMvc(options => options.InputFormatters.Add(new TextPlainInputFormatter()));
		}
		public override void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting envSetting, ILogger<Startup> logger) {
			base.Configure(app, programSetting, envSetting, logger);
		}
		protected override void ConfigureCors(CorsPolicyBuilder builder) {
			base.ConfigureCors(builder);
		}
	}
}