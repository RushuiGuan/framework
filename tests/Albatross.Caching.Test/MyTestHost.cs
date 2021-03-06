﻿using Albatross.Hosting.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Albatross.Caching.Test {
	public class MyTestHost : TestHost {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddCaching();
			services.AddCacheMgmt(typeof(MyCacheMgmt).Assembly);
		}

		public override async Task InitAsync(IConfiguration configuration, ILogger logger) {
			await base.InitAsync(configuration, logger);
			this.Provider.UseCache();

		}
	}
}
