using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Eventing.Pub {
	public class PublisherReplayMessageGroup {
		public int Index { get; set; }
		public IMessage Request { get; init; }
		public PublisherReplayMessageGroup(IMessage request, int index) { 
			this.Request = request;
			this.Index = index;
		}
		public ClientAck? Ack { get; set; }
	}
	public class PublisherReplayService : IRouterServerService {
		private readonly IPublisherService publisherService;
		private readonly ILogger<PublisherReplayService> logger;
		private Dictionary<string, PublisherReplayMessageGroup> records = new Dictionary<string, PublisherReplayMessageGroup>();
		public static string GetKey(IMessage messsage) => $"{messsage.Route}.{messsage.Id}";

		public bool CanReceive => false;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => false;

		public PublisherReplayService(IPublisherService publisherService, ILogger<PublisherReplayService> logger) {
			this.publisherService = publisherService;
			this.logger = logger;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) => throw new NotSupportedException();
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) => throw new NotSupportedException();
		public bool ProcessQueue(IMessagingService messagingService, object msg) {
			switch (msg) {
				case StartReplay _:
					records.Clear();
					break;
				case Replay replay:
					return AcceptReplay(replay);
				case EndReplay _:
					End(messagingService);
					break;
			}
			return false;
		}
		bool AcceptReplay(Replay replay) {
			var key = GetKey(replay.Message);
			switch (replay.Message) {
				case SubscriptionRequest sub:
					var record = new PublisherReplayMessageGroup(sub, replay.Index);
					records[key] = record;
					break;
				case Event eve:
					record = new PublisherReplayMessageGroup(eve, replay.Index);
					records[key] = record;
					break;
				case ClientAck ack:
					if (records.TryGetValue(key, out var value)) {
						value.Ack = ack;
						if(value.Request is Event) {
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
			logger.LogInformation("rerun {count} publisher messages to replay", records.Count);
			foreach (var msg in records.Values.OrderBy(args => args.Index)) {
				if(msg.Request is SubscriptionRequest) {
					publisherService.ProcessReceivedMsg(messagingService, msg.Request);
				} else if(msg.Request is Event){
					messagingService.Transmit(msg.Request);
				}
			}
			logger.LogInformation("publisher replay completed");
		}
	}
}
