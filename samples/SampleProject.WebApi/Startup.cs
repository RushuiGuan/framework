using Albatross.Config;
using Albatross.Hosting;
using Albatross.Messaging.Configurations;
using Albatross.Messaging.Eventing;
using Albatross.Messaging.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SampleProject.WebApi {
	public class Startup : Albatross.Hosting.Startup {
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;
		public override bool Spa => false;
		public override bool Caching => true;


		public Startup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			//services.AddDefaultSampleProjectClient();
			services.AddCustomSampleProjectClient();
			services.AddSingleton<ISubscriber, MySubscriber>();
			 //services.AddMvc(options => options.InputFormatters.Add(new TextPlainInputFormatter()));
			 services.AddControllers(options => options.InputFormatters.Add(new PlainTextInputFormatter()));
		}

		public override void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting environmentSetting, ILogger<Albatross.Hosting.Startup> logger) {
			base.Configure(app, programSetting, environmentSetting, logger);
			// app.ApplicationServices.UseDefaultSampleProjectClient();
			app.ApplicationServices.UseCustomSampleProjectClient();
		}
	}
}
