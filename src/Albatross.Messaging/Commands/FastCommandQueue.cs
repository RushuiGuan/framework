using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// the command queue will run fully on the netmq main thread if the handler itself has no async code.  Otherwise, the all code prior the 
	/// handler invoke will run on the netmq thread.  This implementation is not as well tested as its base class
	/// </summary>
	public class FastCommandQueue : CommandQueue {
		public FastCommandQueue(RouterServer routerServer, IServiceScopeFactory scopeFactory) : base(routerServer, scopeFactory) {
		}

		public async override Task Run(CommandJob job) {
			try {
				logger.LogInformation("Running => {id}", job.Id);
				using var scope = scopeFactory.CreateScope();
				var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(job.Registration.CommandHandlerType);
				var result = await commandHandler.Handle(job.Command, this.Name).ConfigureAwait(false);
				logger.LogInformation("end: {commandId}", job.Id);

				if (job.Registration.HasReturnType) {
					var stream = new MemoryStream();
					JsonSerializer.Serialize(stream, result, job.Registration.ResponseType, MessagingJsonSettings.Value.Default);
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