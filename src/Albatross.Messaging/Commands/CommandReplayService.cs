using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Commands {
	public class CommandReplayService : IRouterServerService {
		Dictionary<string, CommandReplayMessageGroup> records = new Dictionary<string, CommandReplayMessageGroup>();
		private readonly ILogger<CommandReplayService> logger;
		private readonly ICommandBus commandBus;
		public static string GetKey(IMessage messsage) => $"{messsage.Route}.{messsage.Id}";

		public bool CanReceive => false;
		public bool CanTransmit => true;
		public bool NeedTimer => false;

		public CommandReplayService(ILogger<CommandReplayService> logger, ICommandBus commandBus) {
			this.logger = logger;
			this.commandBus = commandBus;
		}

		bool Accept( Replay replay) {
			string key = GetKey(replay.Message);
			switch (replay.Message) {
				case CommandRequest req:
					var record = new CommandReplayMessageGroup(req, replay.Index);
					records[key] = record;
					break;
				case CommandReply rep:
					if (records.TryGetValue(key, out var value)) {
						value.Response = rep;
					}
					break;
				case CommandErrorReply err:
					if (records.TryGetValue(key, out value)) {
						value.Response = err;
					}
					break;
				case CommandExecuted exec:
					if (records.TryGetValue(key, out value)) {
						value.Executed = exec;
						if (value.IsCompleted) {
							records.Remove(key);
						}
					}
					break;
				case Ack ack:
					if (records.TryGetValue(key, out value)) {
						value.Ack = ack;
						if (value.IsCompleted) {
							records.Remove(key);
						}
						return true;
					} else {
						return false;
					}
				default:
					return false;
			}
			return true;
		}
		void End(IMessagingService messagingService) {
			logger.LogInformation("rerun {count} messages after replay", records.Count);
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
	}
}
