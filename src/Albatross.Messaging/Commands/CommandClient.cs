using Albatross.Messaging.Services;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClient {
		Task Submit(object command, bool fireAndForget = true);
	}
	public class CommandClient : ICommandClient , IDisposable{
		private readonly CommandClientService service;
		private readonly IMessagingService dealerClient;

		public CommandClient(IMessagingService dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
		}
		public Task Submit(object command, bool fireAndForget = true)
			=> service.Submit(dealerClient, command, fireAndForget);

		public void Dispose() {
			throw new NotImplementedException();
		}
	}

	public abstract class CallbackCommandClient : ICommandClient , IDisposable{
		private readonly CommandClientService service;
		private readonly IMessagingService dealerClient;

		public abstract void OnCommandCallback(ulong id, string commandType, byte[] message);
		public abstract void OnCommandErrorCallback(ulong id, string commandType, string errorType, byte[] message);

		public CallbackCommandClient(IMessagingService dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
			service.OnCommandCompleted += OnCommandCallback;
			service.OnCommandError += OnCommandErrorCallback;
		}
		
		public ulong Submit(object command, bool fireAndForget = true)
			=> service.Submit(dealerClient, command, fireAndForget);

		public void Dispose() {
			this.service.OnCommandCompleted -= OnCommandCallback;
			this.service.OnCommandError -= OnCommandErrorCallback;
		}
	}
}