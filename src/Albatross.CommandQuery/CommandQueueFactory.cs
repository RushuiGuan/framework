using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Albatross.CommandQuery {
	public interface ICommandQueueFactory :IDisposable  {
		void Submit(Command command);
	}

	public class CommandQueueFactory : ICommandQueueFactory {
		protected readonly IServiceScopeFactory scopeFactory;
		protected readonly ILogger<DefaultCommandQueue> logger;
		private readonly ConcurrentDictionary<string, ICommandQueue> dict = new ConcurrentDictionary<string, ICommandQueue>();

		public CommandQueueFactory(IServiceScopeFactory scopeFactory, ILogger<DefaultCommandQueue> logger) {
			this.scopeFactory = scopeFactory;
			this.logger = logger;
		}

		public virtual ICommandQueue CreateQueue(Command command) {
			return new DefaultCommandQueue(this.scopeFactory, logger);
		}

		public void Submit(Command command) {
			var queue = dict.GetOrAdd(command.QueueName, (key) => CreateQueue(command));
			queue.Submit(command);
		}

		public void Dispose() {
			foreach (var key in this.dict.Keys) {
				dict.Remove(key, out var queue);
				queue?.Dispose();
			}
		}
	}
}
