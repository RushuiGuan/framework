using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {

	public class CommandClient : ICommandClient {
		private readonly CommandClientService service;
		private readonly DealerClient dealerClient;

		public CommandClient(CommandClientService service, DealerClient dealerClient) {
			this.service = service;
			this.dealerClient = dealerClient;
		}
		public Task Ping() => service.Ping(dealerClient);
		public Task<CommandQueueInfo[]> QueueStatus() => service.QueueStatus(dealerClient);
		public Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : notnull
			where ResponseType : notnull => service.Submit<CommandType, ResponseType>(dealerClient, command);

		public Task Submit<CommandType>(CommandType command, bool fireAndForget) where CommandType : notnull
			=> service.Submit<CommandType>(dealerClient, command, fireAndForget);
	}
}