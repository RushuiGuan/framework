using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Commands {
	public class CommandBusReplayService : IRouterServerService {
		Dictionary<string, CommandReplayMessageGroup> records = new Dictionary<string, CommandReplayMessageGroup>();
		private readonly ILogger<CommandBusReplayService> logger;
		private readonly ICommandBusService commandBus;
		public static string GetKey(IMessage messsage) => $"{messsage.Route}.{messsage.Id}";

		public bool CanReceive => false;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => false;

		public CommandBusReplayService(ILogger<CommandBusReplayService> logger, ICommandBusService commandBus) {
			this.logger = logger;
			this.commandBus = commandBus;
		}

		bool Accept(Replay replay) {
			string key = GetKey(replay.Message);
			switch (replay.Message) {
				case CommandRequest req:
					var record = new CommandReplayMessageGroup(req, replay.Index);
					records[key] = record;
					return true;
				case CommandReply rep:
					if (records.TryGetValue(key, out var value)) {
						value.Response = rep;
					}
					return true;
				case CommandErrorReply err:
					if (records.TryGetValue(key, out value)) {
						value.Response = err;
					}
					return true;
				case CommandExecuted exec:
					if (records.TryGetValue(key, out value)) {
						value.Executed = exec;
						if (value.IsCompleted) {
							records.Remove(key);
						}
					}
					return true;
				case ClientAck ack:
					if (records.TryGetValue(key, out value)) {
						value.Ack = ack;
						if (value.IsCompleted) {
							records.Remove(key);
						}
						return true;
					} else {
						return false;
					}
			}
			return false;
		}
		void End(IMessagingService messagingService) {
			logger.LogInformation("rerun {count} commandbus messages to replay", records.Count);
			foreach (var msg in records.Values.OrderBy(args => args.Index)) {
				if (msg.Request.FireAndForget) {
					//for fire and forget commands.  missing executed record means that it was not executed
					if (msg.Executed == null) {
						this.commandBus.ProcessReceivedMsg(messagingService, msg.Request);
					}
				} else {
					if (msg.Response == null) {
						this.commandBus.ProcessReceivedMsg(messagingService, msg.Request);
					} else if (msg.Ack == null) {
						messagingService.Transmit(msg.Response);
					}
				}
			}
			logger.LogInformation("command bus replay completed");
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) => throw new NotSupportedException();

		public bool ProcessTransmitQueue(IMessagingService messagingService, object msg) {
			switch (msg) {
				case StartReplay _:
					this.records.Clear();
					break;
				case Replay replay:
					return Accept(replay);
				case EndReplay _:
					this.End(messagingService);
					break;
			}
			return false;
		}
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) { }
	}
}
