using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.CommandQuery {
	public class DefaultEventPublisher<T> : IEventPublisher<T> {
		private readonly IEnumerable<IEventSubscription<T>> subscriptions;
		private readonly ILogger<DefaultEventPublisher<T>> logger;

		public DefaultEventPublisher(IEnumerable<IEventSubscription<T>> subscriptions, ILogger<DefaultEventPublisher<T>> logger) {
			this.subscriptions = subscriptions;
			this.logger = logger;
		}

		public async Task Send(T @event) {
			logger.LogInformation("publishing event: {@event}", @event);
			foreach(var sub in subscriptions) {
				try {
					await sub.Receive(@event).ConfigureAwait(false);
				}catch(Exception err) {
					logger.LogError(err, "error publishing event to {subscriber}\n{@data}", sub.Subscriber, @event);
				}
			}
		}
	}
}
