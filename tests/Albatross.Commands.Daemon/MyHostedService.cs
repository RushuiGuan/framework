using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class MyHostedService : IHostedService {
		public MyHostedService() {
		}

		public Task StartAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}
	}
}
