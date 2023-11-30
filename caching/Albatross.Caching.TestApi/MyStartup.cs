﻿using Albatross.Caching.MemCache;
using Albatross.Caching.Redis;
using Albatross.Config;
using Albatross.Hosting;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Albatross.Caching.TestApi {
	public class MyStartup : Startup {
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
			services.AddMemCachingAsSecondary();
			services.AddSignalR();
			services.AddControllers(options => options.InputFormatters.Add(new PlainTextInputFormatter()));
		}
		public override void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting envSetting, ILogger<Startup> logger) {
			base.Configure(app, programSetting, envSetting, logger);
			app.ApplicationServices.UseRedisCaching();
		}
		protected override void ConfigureCors(CorsPolicyBuilder builder) {
			base.ConfigureCors(builder);
		}
	}
}