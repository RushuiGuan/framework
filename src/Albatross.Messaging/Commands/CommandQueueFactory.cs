using Albatross.Collections;
using Albatross.Reflection;
using Albatross.Messaging.Commands.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Albatross.Messaging.Commands {
	public interface ICommandQueueFactory {
		CommandJob CreateJob(CommandRequest request);
		IEnumerable<CommandQueueInfo> QueueStatus();
	}

	public class CommandQueueFactory : ICommandQueueFactory {
		protected readonly Dictionary<string, IRegisterCommand> registrations = new Dictionary<string, IRegisterCommand>();
		private readonly Dictionary<string, CommandQueue> commandQueues = new Dictionary<string, CommandQueue>();
		private readonly IServiceProvider provider;


		public CommandQueueFactory(IEnumerable<IRegisterCommand> registrations, IServiceProvider provider) {
			foreach (var item in registrations) {
				this.registrations.Add(item.CommandType.GetClassNameNeat(), item);
			}
			this.provider = provider;
		}

		public CommandJob CreateJob(CommandRequest request) {
			if (registrations.TryGetValue(request.CommandType, out var registration)) {
				var command = JsonSerializer.Deserialize(request.Payload, registration.CommandType, MessagingJsonSettings.Value.Default)
					?? throw new InvalidOperationException($"cannot deserialize command object of type {registration.CommandType.FullName} for {request.Id}");
				var queueName = registration.GetQueueName(command, provider);
				var queue = this.commandQueues.GetOrAdd(queueName, () => {
					var item = provider.GetRequiredService<CommandQueue>();
					item.SetNewLogger(queueName, provider.GetRequiredService<ILoggerFactory>().CreateLogger($"queue-{queueName}"));
					return item;
				});
				var job = new CommandJob(request, queue, registration, command);
				return job;
			} else {
				throw new InvalidOperationException($"registration not found for command type {request.CommandType}");
			}
		}

		public IEnumerable<CommandQueueInfo> QueueStatus() => this.commandQueues.Select(args => new CommandQueueInfo(args.Key, args.Value.Count)).ToArray();
	}
}
