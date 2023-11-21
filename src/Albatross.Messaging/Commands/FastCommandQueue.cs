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

		public async override Task Run(CommandQueueItem item) {
			try {
				logger.LogDebug("Running => {id}", item.Id);
				using var scope = scopeFactory.CreateScope();
				var commandHandler = (ICommandHandler)scope.ServiceProvider.GetRequiredService(item.Registration.CommandHandlerType);
				var result = await commandHandler.Handle(item.Command, this.Name).ConfigureAwait(false);
				logger.LogInformation("end: {commandId}", item.Id);

				if (item.Registration.HasReturnType) {
					var stream = new MemoryStream();
					JsonSerializer.Serialize(stream, result, item.Registration.ResponseType, MessagingJsonSettings.Value.Default);
					item.Reply = new CommandReply(item.Route, item.Id, item.CommandType, stream.ToArray());
				} else {
					item.Reply = new CommandReply(item.Route, item.Id, item.CommandType, Array.Empty<byte>());
				}
				routerServer.SubmitToQueue(item);
			} catch (Exception err) {
				logger.LogError(err, "error running command {id}", item.Id);
				item.Reply = new CommandErrorReply(item.Route, item.Id, item.CommandType, err.GetType().FullName ?? "Error", err.Message.ToUtf8Bytes());
				routerServer.SubmitToQueue(item);
			}
		}
	}
}