using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClient {
		Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : notnull where ResponseType : notnull;

		Task Submit<CommandType>(CommandType command, bool fireAndForget = true) where CommandType : notnull;
		Task<CommandQueueInfo[]> QueueStatus();
		Task Ping();
	}

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