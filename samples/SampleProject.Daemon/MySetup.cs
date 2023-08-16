﻿using Albatross.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SampleProject.Daemon {
	public class MySetup : Setup {
		public MySetup(string[] args) : base(args) {
		}

		public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
			base.ConfigureServices(services, configuration);
			services.AddSampleProjectCommandBus();
		}
	}
}