using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Albatross.CommandQuery {
	public interface ICommandQueueFactory<T> :IDisposable where T:Command {
		void Submit(T command);
	}

	public class CommandQueueFactory<T> : ICommandQueueFactory<T> where T:Command {
		protected readonly IServiceScopeFactory scopeFactory;
		protected readonly ILogger<CommandQueue<T>> logger;
		private readonly ConcurrentDictionary<string, ICommandQueue<T>> dict = new ConcurrentDictionary<string, ICommandQueue<T>>();

		public CommandQueueFactory(IServiceScopeFactory scopeFactory, ILogger<CommandQueue<T>> logger) {
			this.scopeFactory = scopeFactory;
			this.logger = logger;
		}

		public virtual ICommandQueue<T> CreateQueue(T command) {
			string name = GetQueueName(command);
			return new CommandQueue<T>(name, args=> { }, this.scopeFactory, logger);
		}

		public virtual string GetQueueName(T command) => typeof(T).Name;

		public void Submit(T command) {
			var queue = dict.GetOrAdd(GetQueueName(command), (key) => CreateQueue(command));
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
