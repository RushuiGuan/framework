using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Albatross.Messaging.Commands {
	public interface ICommandBusService : IRouterServerService { }
	public class CommandBusService : ICommandBusService {
		private readonly ICommandQueueFactory commandQueueFactory;
		private readonly ILogger<CommandBusService> logger;
		private readonly MessagingJsonSerializationOption jsonSerializationOption;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => false;

		public CommandBusService(ICommandQueueFactory commandQueueFactory, ILogger<CommandBusService> logger, MessagingJsonSerializationOption jsonSerializationOption) {
			this.commandQueueFactory = commandQueueFactory;
			this.logger = logger;
			this.jsonSerializationOption = jsonSerializationOption;
		}

		private void ReplyCommandQueueStatus(IMessagingService messagingService, CommandQueueStatus status) {
			var result = this.commandQueueFactory.QueueStatus();
			using var stream = new MemoryStream();
			JsonSerializer.Serialize<IEnumerable<CommandQueueInfo>>(stream, result, this.jsonSerializationOption.Default);
			messagingService.SubmitToQueue(new CommandQueueStatusReply(status.Route, status.Id, stream.ToArray()));
		}

		void AcceptRequest(IMessagingService messagingService, CommandRequest cmd) {
			try {
				var job = this.commandQueueFactory.CreateJob(cmd);
				job.Queue.Submit(job);
				// internal route could be sent here due to replay, make sure it doesn't actually get sent to the socket
				if (cmd.FireAndForget && cmd.Route != InternalCommand.Route) {
					// ack the request and be done with it
					messagingService.Transmit(new CommandRequestAck(cmd.Route, cmd.Id));
				}
			} catch(Exception err) {
				var msg = new CommandErrorReply(cmd.Route, cmd.Id, err.GetType().FullName ?? "Error", err.Message.ToUtf8Bytes());
				messagingService.SubmitToQueue(msg);
			}
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			switch (msg) {
				case CommandRequest cmd:
					this.AcceptRequest(messagingService, cmd);
					return true;
				case PingRequest ping:
					messagingService.SubmitToQueue(new PingReply(ping.Route, ping.Id));
					return true;
				case CommandQueueStatus status:
					this.ReplyCommandQueueStatus(messagingService, status);
					return true;
				default:
					return false;
			}
		}

		public bool ProcessTransmitQueue(IMessagingService messagingService, object msg) {
			switch (msg) {
				case CommandJob job:
					if (job.FireAndForget) {
						messagingService.DataLogger.WriteLogEntry(new DataLogging.LogEntry(LineType.Record, new CommandExecuted(job.Route, job.Id)));
					} else {
						messagingService.SubmitToQueue(job.Reply ?? new CommandErrorReply(job.Route, job.Id, "Error", "reply mia".ToUtf8Bytes()));
					}
					job.Queue.RunNextIfAvailable();
					break;
				case InternalCommand internalCommand:
					try {
						var newJob = this.commandQueueFactory.CreateJob(internalCommand.Request);
						newJob.Queue.Submit(newJob);
						internalCommand.SetResult();
					}catch(Exception e) {
						internalCommand.SetException(e);
					}
					break;
				default:
					return false;
			}
			return true;
		}
		public void ProcessTimerElapsed(IMessagingService routerServer) { }
	}
}
