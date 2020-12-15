﻿using Albatross.Caching;
using Albatross.Hosting.Demo.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
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

		public MyStartup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCaching();
			services.AddSingleton<ICacheManagement, IssuerCachedMgmt>();
			services.AddSingleton<ICacheManagement, SymbolCacheManagement>();
			services.AddSignalR();
		}
		public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
			base.Configure(app, env, logger);
			app.UseEndpoints(buider => {
				buider.MapHub<NotifHub>("/notif");
			});
		}
	}
}