using Albatross.Collections;
using Albatross.Messaging.Services;
using Albatross.Threading;
using System;
using System.Collections.Generic;
using System.Text.Json;
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

	public interface ITaskCallbackCommandClient {
		Task<ulong> SubmitWithCallback(object command, int timeout = 2000);
		Task<T?> SubmitWithCallback<T>(object command, int timeout = 2000);
	}

	public class TaskCallbackCommandClient : CallbackCommandClient, ITaskCallbackCommandClient {
		Dictionary<ulong, TaskCallback<byte[]>> callbacks = new Dictionary<ulong, TaskCallback<byte[]>>();
		object sync = new object();

		TaskCallback<byte[]> GetCallback(ulong id) {
			TaskCallback<byte[]>? callback;
			lock (sync) {
				if (!callbacks.TryGetAndRemove(id, out callback)) {
					callback = new TaskCallback<byte[]>();
					callbacks.Add(id, callback);
				}
			}
			return callback;
		}

		public async Task<ulong> SubmitWithCallback(object command, int timeout = 2000) {
			ulong id = await base.Submit(command, false, timeout);
			var callback = GetCallback(id);
			await callback.Task;
			return id;
		}
		public async Task<T?> SubmitWithCallback<T>(object command, int timeout = 2000) {
			ulong id = await base.Submit(command, false, timeout);
			var callback = GetCallback(id);
			var data = await callback.Task;
			return JsonSerializer.Deserialize<T>(data, Messaging.MessagingJsonSettings.Value.Default);
		}

		public override void OnCommandCallback(ulong id, string commandType, byte[] message) {
			var callback = GetCallback(id);
			callback.SetResult(message);
		}
		public override void OnCommandErrorCallback(ulong id, string commandType, string errorType, byte[] message) {
			var callback = GetCallback(id);
			callback.SetException(new CommandException(id, errorType, message.ToUtf8String()));
		}
		public TaskCallbackCommandClient(DealerClient dealerClient, CommandClientService service) : base(dealerClient, service) { }
	}
}