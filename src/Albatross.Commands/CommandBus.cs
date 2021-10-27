using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public interface ICommandBus : IDisposable {
		Task<T> Submit<T>(Command<T> command) where T:notnull;
		ICommandQueue Get(string name);
		IEnumerable<string> GetAllQueues();
		bool IsQueueBusy(Regex regex);
	}

	public class CommandBus : ICommandBus {
		private readonly IServiceProvider serviceProvider;
		protected readonly ILogger<CommandBus> logger;
		private readonly ConcurrentDictionary<string, ICommandQueue> queues = new ConcurrentDictionary<string, ICommandQueue>();
		// registration is initialized in constructor and it is immutable, no need to lock
		private readonly Dictionary<Type, IRegisterCommand> registration = new Dictionary<Type, IRegisterCommand>();

		public CommandBus(IServiceProvider serviceProvider, IEnumerable<IRegisterCommand> registration, ILogger<CommandBus> logger) {
			this.serviceProvider = serviceProvider;
			this.logger = logger;

			foreach (var item in registration) {
				this.registration[item.CommandType] = item;
			}
		}

		protected virtual ICommandQueue CreateQueue(string name, Command command) {
			if (registration.TryGetValue(command.GetType(), out IRegisterCommand registered)) {
				var queue = registered.Create(name, this.serviceProvider);
				Task.Run(queue.Start);
				return queue;
			} else {
				throw new ArgumentException($"Command {command.GetType().FullName} is not registered");
			}
		}

		public Task<T> Submit<T>(Command<T> command) where T:notnull{
			if (registration.TryGetValue(command.GetType(), out IRegisterCommand registered)) {
				string name = registered.GetQueueName(command, serviceProvider);
				var queue = queues.GetOrAdd(name, (key) => CreateQueue(key, command));
				queue.Submit(command);
				return command.Task;
			} else {
				throw new ArgumentException($"Command {command.GetType().FullName} is not registered");
			}
		}

		public void Dispose() {
		}

		public ICommandQueue Get(string name) {
			if (this.queues.TryGetValue(name, out var value)) {
				return value;
			} else {
				throw new ArgumentException($"{name} is not a valid command queue");
			}
		}

		public IEnumerable<string> GetAllQueues() => this.queues.Keys;

		public bool IsQueueBusy(Regex regex) {
			foreach(var item in this.queues.ToArray()) {
				if (regex.IsMatch(item.Key) && item.Value.Count > 0) {
					return true;
				}
			}
			return false;
		}
	}
}
