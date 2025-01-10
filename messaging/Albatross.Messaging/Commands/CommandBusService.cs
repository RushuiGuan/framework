using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Albatross.Messaging.Commands {
	public interface ICommandBusService : IRouterServerService { }
	public class CommandBusService : ICommandBusService {
		private readonly ICommandQueueFactory commandQueueFactory;
		private readonly ILogger<CommandBusService> logger;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => false;

		public CommandBusService(ICommandQueueFactory commandQueueFactory, ILogger<CommandBusService> logger) {
			this.commandQueueFactory = commandQueueFactory;
			this.logger = logger;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			if (msg is CommandRequest cmd) {
				try {
					var item = this.commandQueueFactory.CreateItem(cmd);
					logger.LogDebug("AcceptingRequest => {id}", item.Id);
					item.Queue.Submit(item, false);
					// internal command should not be routed here
					Debug.Assert(item.Mode != CommandMode.Internal);
					messagingService.Transmit(new CommandRequestAck(cmd.Route, cmd.Id));
				} catch (Exception err) {
					var errMsg = new CommandRequestError(cmd.Route, cmd.Id, cmd.CommandName, err.GetType().FullName ?? "Error", err.Message.ToUtf8Bytes());
					messagingService.SubmitToQueue(errMsg);
				}
				return true;
			} else {
				return false;
			}
		}

		public bool ProcessQueue(IMessagingService messagingService, object msg) {
			switch (msg) {
				case CommandQueueItem item:
					// the CommandQueueItem is sent here when it has finished its execution
					if (item.Reply == null) {
						logger.LogError("CommandQueueItem {type} for {queue} arrived without a reply message: {@command}", item.CommandName, item.Queue, item.Command);
					} else if (item.Mode != CommandMode.Callback) {
						messagingService.EventWriter.WriteEvent(new EventSource.EventEntry(EntryType.Record, item.Reply));
					} else {
						messagingService.SubmitToQueue(item.Reply);
					}
					item.IsCompleted = true;
					// after a job has finished, kick off the next job
					item.Queue.RunNextIfNotBusy();
					break;
				case InternalCommand internalCommand:
					// the internal commands are sent here to be queued for execution
					try {
						// record the internal command as receiving message since it bypass the Socket_ReceiveReady of the router server.
						messagingService.EventWriter.WriteEvent(new EventSource.EventEntry(EntryType.In, internalCommand.Request));
						var newItem = this.commandQueueFactory.CreateItem(internalCommand.Request);
						logger.LogDebug("ProcessQueue, InternalCommand => {id}", newItem.Id);
						newItem.Queue.Submit(newItem, internalCommand.Priority);
						internalCommand.SetResult(newItem.Id);
					} catch (Exception err) {
						if (internalCommand is InternalCommandWithCallback) {
							internalCommand.SetException(err);
						} else {
							logger.LogError(err, "Error submitting internal command {@request}", internalCommand.Request);
						}
					}
					break;
				default:
					return false;
			}
			return true;
		}
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) { }
	}
}