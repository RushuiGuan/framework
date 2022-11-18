using Albatross.Caching;
using Albatross.Config;
using Albatross.Hosting.Demo;
using Albatross.Hosting.Demo.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Albatross.Hosting.Test {
	public class MyStartup : Startup {
		public override bool Grpc => false;
		public override bool Secured => true;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Spa => true;
		public override bool Caching => true;

		public MyStartup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCacheMgmt(this.GetType().Assembly);
			services.AddSignalR();
			services.AddRazorPages().AddRazorRuntimeCompilation();
			services.Configure<MvcRazorRuntimeCompilationOptions>(options => {
				options.FileProviders.Add(new RazorViewPathProvider());
			});
			services.AddMvc(options => options.InputFormatters.Add(new TextPlainInputFormatter()));
		}
		public override void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting envSetting, ILogger<Startup> logger) {
			base.Configure(app, programSetting, envSetting, logger);
			app.UseEndpoints(buider => {
				buider.MapHub<NotifHub>("/notif");
			});
		}
		protected override void ConfigureCors(CorsPolicyBuilder builder) {
			base.ConfigureCors(builder);
		}
	}
}