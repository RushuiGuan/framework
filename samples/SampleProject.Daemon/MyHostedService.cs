using Albatross.Messaging.Commands;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Hosting;
using NetMQ;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleProject.Daemon {
	public class MyHostedService : IHostedService {
		private readonly RouterServer server;

		public MyHostedService(RouterServer server) {
			this.server = server;
		}

		public async Task StartAsync(CancellationToken cancellationToken) {
			await server.Start();
			NetMQConfig.Linger = TimeSpan.FromSeconds(60);
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			server.Dispose();
			NetMQConfig.Cleanup(true);
			return Task.CompletedTask;
		}
	}
}
