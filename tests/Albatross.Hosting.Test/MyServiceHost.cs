﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Hosting.Test {
	public class MyServiceHost : BackgroundService {
		private readonly ILogger<MyServiceHost> logger;

		public MyServiceHost(ILogger<MyServiceHost> logger) {
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			while (true) {
				await Task.Delay(1000, stoppingToken);
				stoppingToken.ThrowIfCancellationRequested();
			}
		}
	}
}