﻿using Albatross.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Daemon {
	public class MySetup : Setup {
		public MySetup(string[] args) : base(args) {
		}

		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			base.ConfigureServices(services, configuration);
			services.AddMessagingDaemonServices();
		}
	}
}