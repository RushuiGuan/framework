using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	// this class is not threadsafe and all methods can only be run by the netmq thread
	public class CommandQueue {
		protected ILogger logger = null!;
		protected readonly RouterServer routerServer;
		protected readonly IServiceScopeFactory scopeFactory;
		protected readonly Queue<CommandQueueItem> priorityQueue = new Queue<CommandQueueItem>();
		protected readonly Queue<CommandQueueItem> queue = new Queue<CommandQueueItem>();
		protected CommandQueueItem? currentItem;
		public string Name { get; private set; } = string.Empty;

		public CommandQueue(RouterServer routerServer, IServiceScopeFactory scopeFactory) {
			this.routerServer = routerServer;
			this.scopeFactory = scopeFactory;
		}
		public void SetNewLogger(string queueName, ILogger logger) {
			this.Name = queueName;
			this.logger = logger;
			logger.LogInformation("command queue {name} setup", queueName);
		}
		public void Submit(CommandQueueItem item, bool priority) {
			logger.LogDebug("Submitting => {id}", item.Id);
			if (priority) {
				this.priorityQueue.Enqueue(item);
			} else {
				this.queue.Enqueue(item);
			}
			this.RunNextIfNotBusy();
		}

		public int Count => this.queue.Count + this.priorityQueue.Count;

		public void RunNextIfNotBusy() {
			if (currentItem == null || currentItem.IsCompleted) {
				if (this.priorityQueue.TryDequeue(out var next) || this.queue.TryDequeue(out next)) {
					logger.LogDebug("RunNextIfNotBusy => {id}", next.Id);
					this.currentItem = next;
					_ = Run(next);
				}
			}
		}
		public async virtual Task Run(CommandQueueItem item) {
			using var scope = scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<CommandContext>();
			item.SetContext(context);
			try {
				var commandHandler = item.Registration.CreateCommandHandler(scope.ServiceProvider);
				// run everything else using a diff thread
				await Task.Run(async () => {
					if (item.Command is ILogDetail logDetail) {
						logger.LogInformation("Running {id}, {client}, {mode}, {type}, parameter:{@parameter}",
							item.Id, item.Route, item.Mode, item.Registration.CommandType, logDetail.Target);
					} else {
						logger.LogInformation("Running {id}, {client}, {mode}, {type}",
							item.Id, item.Route, item.Mode, item.Registration.CommandType);
					}
					var result = await commandHandler.Handle(item.Command).ConfigureAwait(false);
					logger.LogInformation("Done {commandId}", item.Id);

					if (item.Registration.HasReturnType) {
						var stream = new MemoryStream();
						JsonSerializer.Serialize(stream, result, item.Registration.ResponseType, MessagingJsonSettings.Value.Default);
						item.Reply = new CommandReply(item.Route, item.Id, item.CommandName, stream.ToArray());
					} else {
						item.Reply = new CommandReply(item.Route, item.Id, item.CommandName, Array.Empty<byte>());
					}
					routerServer.SubmitToQueue(item);
				});
			} catch (Exception err) {
				item.Reply = new CommandErrorReply(item.Route, item.Id, item.CommandName, err.GetType().FullName ?? "Error", err.Message.ToUtf8Bytes());
				routerServer.SubmitToQueue(item);
				logger.LogError(err, "Failed {commandId}", item.Id);
			}
		}
	}
}