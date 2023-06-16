using Albatross.Reflection;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	/// <summary>
	/// InternalCommandClient allows commands to be routed with the server itself.  It uses a special route called <see cref="InternalCommand.Route"/>.  If used, avoid routes of the
	/// same name.  It only supports FireAndForget command requests because risk of dead lock is high if caller is waiting for a response.
	/// IMPORTANT: this class should only be used if TaskCommandQueue is being used, which is the default setup.  The default implementation of CommandQueue 
	/// could run on the poller thread and block itself when waiting for an Ack.
	/// </summary>
	public class InternalCommandClient : ICommandClient {
		private readonly RouterServer routerServer;
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();
		private readonly MessagingJsonSerializationOption serializationOption;

		public InternalCommandClient(RouterServer routerServer, MessagingJsonSerializationOption serializationOption) {
			this.routerServer = routerServer;
			this.serializationOption = serializationOption;
		}

		public Task Ping() => throw new NotSupportedException();
		public Task<CommandQueueInfo[]> QueueStatus() => throw new NotSupportedException();

		public Task<ResponseType> Submit<CommandType, ResponseType>(CommandType command)
			where CommandType : notnull where ResponseType : notnull {
			throw new NotSupportedException();
		}

		public Task Submit<CommandType>(CommandType command, bool fireAndForget) where CommandType : notnull {
			if (fireAndForget) {
				using var stream = new MemoryStream();
				JsonSerializer.Serialize<CommandType>(stream, command, this.serializationOption.Default);
				var request = new CommandRequest(InternalCommand.Route, counter.NextId(), typeof(CommandType).GetClassNameNeat(), true, stream.ToArray());
				var internalCmd = new InternalCommand(request);
				this.routerServer.SubmitToQueue(internalCmd);
				return internalCmd.Task;
			} else {
				throw new NotSupportedException();
			}
		}
	}
}