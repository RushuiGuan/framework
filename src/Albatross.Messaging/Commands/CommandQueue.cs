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
		protected readonly Queue<CommandQueueItem> queue = new Queue<CommandQueueItem>();
		protected CommandQueueItem? current;
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
		public void Submit(CommandQueueItem job) {
			logger.LogDebug("Submitting => {id}", job.Id);
			this.queue.Enqueue(job);
			this.RunNextIfNotBusy();
		}

		public int Count { get => this.queue.Count; }

		public void RunNextIfNotBusy() {
			if (current == null || current.IsCompleted) {
				if (this.queue.TryDequeue(out var next)) {
					logger.LogDebug("RunNextIfNotBusy => {id}", next.Id);
					this.current = next;
					_ = Run(next);
				}
			}
		}
		public async virtual Task Run(CommandQueueItem job) {
			try {
				using var scope = scopeFactory.CreateScope();
				var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(job.Registration.CommandHandlerType);
				// run everything else using a diff thread
				await Task.Run(async () => {
					if (job.Command is ILogDetail logDetail) {
						logger.LogInformation("Running {command} by {client}({id}), parameter: {@parameter}",
							job.Registration.CommandType, job.Route, job.Id, logDetail.Target);
					} else {
						logger.LogInformation("Running {command} by {client}({id})",
							job.Registration.CommandType, job.Route, job.Id);
					}
					var result = await commandHandler.Handle(job.Command, this.Name).ConfigureAwait(false);
					logger.LogInformation("Done {commandId}", job.Id);

					if (job.Registration.HasReturnType) {
						var stream = new MemoryStream();
						JsonSerializer.Serialize(stream, result, job.Registration.ResponseType, MessagingJsonSettings.Value.Default);
						job.Reply = new CommandReply(job.Route, job.Id, stream.ToArray());
					} else {
						job.Reply = new CommandReply(job.Route, job.Id, Array.Empty<byte>());
					}
					routerServer.SubmitToQueue(job);
				});
			} catch (Exception err) {
				job.Reply = new CommandErrorReply(job.Route, job.Id, err.GetType().FullName ?? "unknown class", err.Message.ToUtf8Bytes());
				routerServer.SubmitToQueue(job);
				logger.LogError(err, "Failed {commandId}", job.Id);
			}
		}
	}
}