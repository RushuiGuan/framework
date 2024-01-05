using Albatross.Threading;
using Albatross.Messaging.Services;
using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// With the exception of the Start and Dispose method, which is used for initialization, 
	/// all other methods in this call should be thread safe.
	/// </summary>
	public interface ICommandClient {
		Task<ulong> Submit(object command, bool fireAndForget = true, int timeout = 2000);
	}
	public class CommandClient : ICommandClient {
		private readonly CommandClientService service;
		private readonly IMessagingService dealerClient;

		public CommandClient(DealerClient dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
		}
		public Task<ulong> Submit(object command, bool fireAndForget = true, int timeout = 2000)
			=> service.Submit(dealerClient, command, fireAndForget).WithTimeOut(TimeSpan.FromMilliseconds(timeout));
	}

	public abstract class CallbackCommandClient : ICommandClient, IDisposable {
		private readonly CommandClientService service;
		private readonly IMessagingService dealerClient;

		public abstract void OnCommandCallback(ulong id, string commandType, byte[] message);
		public abstract void OnCommandErrorCallback(ulong id, string commandType, string errorType, byte[] message);

		public CallbackCommandClient(DealerClient dealerClient, CommandClientService service) {
			this.service = service;
			this.dealerClient = dealerClient;
			service.OnCommandCompleted += OnCommandCallback;
			service.OnCommandError += OnCommandErrorCallback;
		}

		public Task<ulong> Submit(object command, bool fireAndForget = true, int timeout = 2000)
			=> service.Submit(dealerClient, command, fireAndForget).WithTimeOut(TimeSpan.FromMilliseconds(timeout));

		public void Dispose() {
			this.service.OnCommandCompleted -= OnCommandCallback;
			this.service.OnCommandError -= OnCommandErrorCallback;
		}
	}
}