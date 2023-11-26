using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Albatross.Eventing.SignalR {
	public class SignalREventSource : IEventSource, IAsyncDisposable {
		ServiceHubContext? context;
		private readonly ILogger logger;
		private readonly ILoggerFactory loggerFactory;

		public SignalREventSource(ILoggerFactory loggerFactory) {
			this.loggerFactory = loggerFactory;
			this.logger = this.loggerFactory.CreateLogger<SignalREventSource>();
		}

		public async Task Init(string hub, IConfiguration configuration, ILoggerFactory loggerFactory, CancellationToken cancellationToken) {
			if (context == null) {
				logger.LogInformation("create new signalr service manager builder");
				using var serviceManager = new ServiceManagerBuilder().WithConfiguration(configuration).WithLoggerFactory(loggerFactory).BuildServiceManager();
				this.context = await serviceManager.CreateHubContextAsync(hub, cancellationToken);
			}
		}

		public async ValueTask DisposeAsync() {
			if (this.context != null) {
				await this.context.DisposeAsync();
				this.context = null;
			}
		}

		public async Task Send<T>(ClientType clientType, string client, string topic, object[] data, CancellationToken cancellationToken) {
			if (context == null) {
				throw new InvalidOperationException("ServiceHubContext has not been initialized");
			} else {
				if (clientType == ClientType.All) {
					await context.Clients.All.SendCoreAsync(topic, data);
				} else if (clientType == ClientType.Group) {
					await context.Clients.Group(client).SendCoreAsync(topic, data);
				} else if (clientType == ClientType.User) {
					await context.Clients.User(client).SendCoreAsync(topic, data);
				}
			}
		}
	}
}
