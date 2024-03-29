﻿using Albatross.Threading;
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
		private readonly CommandContext context;

		public InternalCommandClient(RouterServer routerServer, CommandContext context) {
			this.routerServer = routerServer;
			this.context = context;
		}
		public Task<ulong> Submit(object command, bool fireAndForget = true, int timeout = 2000) {
			var type = command.GetType();
			if (fireAndForget) {
				using var stream = new MemoryStream();
				JsonSerializer.Serialize(stream, command, type, MessagingJsonSettings.Value.Default);
				// internal commands have the route of "internal" and can use the ids of the router server
				var id = routerServer.Counter.NextId();
				context.InternalCommands.Add(id);
				var request = new CommandRequest(context.Route, id, type.GetClassNameNeat(), CommandMode.Internal, stream.ToArray());
				var internalCmd = new InternalCommandWithCallback(request);
				this.routerServer.SubmitToQueue(internalCmd);
				return internalCmd.Task.WithTimeOut(TimeSpan.FromMilliseconds(timeout));
			} else {
				throw new NotSupportedException();
			}
		}
	}
}