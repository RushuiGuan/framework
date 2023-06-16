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
		public bool CanTransmit => true;
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
				if (cmd.FireAndForget) {
					// ack the request and be done with it
					messagingService.Transmit(new CommandRequestAck(cmd.Route, cmd.Id));
				}
			} catch(Exception err) {
				var msg = new CommandErrorReply(cmd.Route, cmd.Id, err.GetType().FullName ?? "Error", err.Message);
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
					if (!job.FireAndForget) {
						messagingService.SubmitToQueue(job.Reply ?? new CommandErrorReply(job.Route, job.Id, "Error", "reply mia"));
					} else {
						messagingService.DataLogger.Record(new CommandExecuted(job.Route, job.Id));
					}
					job.Queue.RunNextIfAvailable();
					return true;
				default:
					return false;
			}
		}
	}
}
