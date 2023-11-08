using Albatross.Messaging.Services;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClient {
		ulong Submit(object command, bool fireAndForget = true);
	}
	public class CommandClient : ICommandClient {
		private readonly CommandClientService service;
		private readonly DealerClient dealerClient;

		public CommandClient(DealerClient dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
		}
		public ulong Submit(object command, bool fireAndForget = true)
			=> service.Submit(dealerClient, command, fireAndForget);
	}
}