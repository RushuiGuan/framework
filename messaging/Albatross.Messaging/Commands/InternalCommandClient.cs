using Albatross.Messaging.Core;
using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Services;
using Albatross.Threading;
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
		private readonly CommandContext context;

		public InternalCommandClient(RouterServer routerServer, CommandContext context) {
			this.routerServer = routerServer;
			this.context = context;
		}
		/// <summary>
		/// The InternalCommandClient can only submit commands that are fire and forget.  If the timeout is set and the caller awaits the result, 
		/// the caller is waiting to ensure that the command has been received and processed by the router server queue. In the rare situation where 
		/// the router server has a large amount of items on its queue, the returning task could wait for a while.  That is why a timeout value is 
		/// required.  Keep in mind that the timeout is always a possibility.  If the timeout of 0 is set, await the task will return the id of the command
		/// immediately.  The caller will not be waiting for anything.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="fireAndForget"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException"></exception>
		public Task<ulong> Submit(object command, bool _ = false, int timeout = 0) {
			return Send(command, false, timeout);
		}
		public Task<ulong> SubmitPriority(object command, int timeout = 0) {
			return Send(command, true, timeout);
		}

		private Task<ulong> Send(object command, bool priority, int timeout = 0) {
			var type = command.GetType();
			using var stream = new MemoryStream();
			JsonSerializer.Serialize(stream, command, type, MessagingJsonSettings.Value.Default);
			// internal commands have the route of "internal" and can use the ids of the router server
			var id = routerServer.Counter.NextId();
			context.InternalCommands.Add(id);
			var request = new CommandRequest(context.Route, id, type.GetCommandName(), CommandMode.Internal, stream.ToArray());
			if (timeout == 0) {
				var internalCmd = new InternalCommand(request) {
					Priority = priority,
				};
				this.routerServer.SubmitToQueue(internalCmd);
				return Task.FromResult(id);
			} else {
				var internalCmd = new InternalCommandWithCallback(request) {
					Priority = priority,
				};
				this.routerServer.SubmitToQueue(internalCmd);
				return internalCmd.Task.WithTimeOut(TimeSpan.FromMilliseconds(timeout));
			}
		}
	}
}