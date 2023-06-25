﻿using Albatross.Messaging.Commands.Messages;
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
		protected readonly MessagingJsonSerializationOption jsonSerializationOption;
		protected readonly Queue<CommandJob> queue = new Queue<CommandJob>();
		protected Task? current;

		public CommandQueue(RouterServer routerServer, IServiceScopeFactory scopeFactory, MessagingJsonSerializationOption jsonSerializationOption) {
			this.routerServer = routerServer;
			this.scopeFactory = scopeFactory;
			this.jsonSerializationOption = jsonSerializationOption;
		}
		public void SetNewLogger(string queueName, ILogger logger) {
			this.logger = logger;
			logger.LogInformation("command queue {name} setup", queueName);
		}
		public void Submit(CommandJob job) {
			this.queue.Enqueue(job);
			this.RunNextIfNotBusy();
		}

		public int Count { get => this.queue.Count; }

		public void RunNextIfNotBusy() {
			if (current == null || current.IsCompleted == true) {
				if (this.queue.TryDequeue(out var next)) {
					this.current = Run(next);
				}
			}
		}
		/// <summary>
		/// this call doesn't care if the queue is busy.  should only be called by the run function itself.
		/// it is should still be called on the netmq thread, therefore not invoked by the Run method.
		/// if this method doesn't kick off a new command, it will set the current to null.  This garantees
		/// the next RunNext call to kick of a new command if the queue is not empty
		/// </summary>
		internal void RunNextIfAvailable() {
			if (this.queue.TryDequeue(out var next)) {
				this.current = Run(next);
			} else {
				this.current = null;
			}
		}

		public async virtual Task Run(CommandJob job) {
			try {
				using var scope = scopeFactory.CreateScope();
				var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(job.Registration.CommandHandlerType);
				// logger.LogInformation("start: {commandId}", message.MessageId);
				var result = await commandHandler.Handle(job.Command).ConfigureAwait(false);
				logger.LogInformation("end: {commandId}", job.Id);

				if (job.Registration.HasReturnType) {
					var stream = new MemoryStream();
					JsonSerializer.Serialize(stream, result, job.Registration.ResponseType, jsonSerializationOption.Default);
					job.Reply = new CommandReply(job.Route, job.Id, stream.ToArray());
				} else {
					job.Reply = new CommandReply(job.Route, job.Id, Array.Empty<byte>());
				}
				routerServer.SubmitToQueue(job);
			} catch (Exception err) {
				logger.LogError(err, "error running command {id}", job.Id);
				job.Reply = new CommandErrorReply(job.Route, job.Id, err.GetType().FullName ?? "Error", err.Message.ToUtf8Bytes());
				routerServer.SubmitToQueue(job);
			}
		}
	}
}