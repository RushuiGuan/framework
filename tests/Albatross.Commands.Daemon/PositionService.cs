using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Commands.Daemon {
	public class PositionService : IEventSubscription<MyCommandExecuted> {
		private readonly ILogger<PositionService> logger;

		public PositionService(ILogger<PositionService> logger) {
			this.logger = logger;
		}

		public string Subscriber => "Position Service";

		public async Task Receive(MyCommandExecuted @event) {
			logger.LogInformation("receiving event {@event}", @event);
			//await Task.Delay(3000);
			logger.LogInformation("received event {@event}", @event);
		}
	}
}
