﻿using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	// this class is not threadsafe and all methods can only be run by the netmq thread
	public class TaskCommandQueue : CommandQueue {
		public TaskCommandQueue(RouterServer routerServer, IServiceScopeFactory scopeFactory) : base(routerServer, scopeFactory) {
		}

		public async override Task Run(CommandJob job) {
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