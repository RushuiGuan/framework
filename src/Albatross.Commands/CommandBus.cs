using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public interface ICommandBus : IDisposable {
		void Submit(Command command);
	}

	public class CommandBus : ICommandBus {
		private readonly IServiceProvider serviceProvider;
		protected readonly ILogger<CommandQueue> logger;
		private readonly ConcurrentDictionary<string, ICommandQueue> dict = new ConcurrentDictionary<string, ICommandQueue>();
		// registration is initialized in constructor and it is immutable, no need to lock
		private readonly Dictionary<Type, IRegisterCommand> registration = new Dictionary<Type, IRegisterCommand>();

		public CommandBus(IServiceProvider serviceProvider, IEnumerable<IRegisterCommand> registration, ILogger<CommandQueue> logger) {
			this.serviceProvider = serviceProvider;
			this.logger = logger;

			foreach (var item in registration) {
				this.registration[item.CommandType] = item;
			}
		}

		protected virtual ICommandQueue CreateQueue(string name, Command command) {
			if (registration.TryGetValue(command.GetType(), out IRegisterCommand? registered)) {
				var queue = (ICommandQueue)serviceProvider.GetRequiredService(registered.QueueType);
				queue.SetName(name);
				Task.Run(queue.Start);
				return queue;
			} else {
				throw new ArgumentException($"Command {command.GetType().FullName} is not registered");
			}
		}

		public void Submit(Command command) {
			if (registration.TryGetValue(command.GetType(), out IRegisterCommand? registered)) {
				string name = registered.GetQueueName(command);
				var queue = dict.GetOrAdd(name, (key) => CreateQueue(key, command));
				queue.Submit(command);
			} else {
				throw new ArgumentException($"Command {command.GetType().FullName} is not registered");
			}
		}

		public void Dispose() {
			foreach (var key in this.dict.Keys) {
				dict.Remove(key, out var queue);
				queue?.Dispose();
			}
		}
	}
}
