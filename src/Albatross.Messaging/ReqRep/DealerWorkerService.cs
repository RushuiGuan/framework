using Albatross.Messaging.Configurations;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Albatross.Messaging.Services;
using Albatross.Messaging.ReqRep.Messages;

namespace Albatross.Messaging.ReqRep {
	public class DealerWorkerService : IDealerClientService {
		private readonly ILogger<DealerWorkerService> logger;
		private readonly IAtomicCounter<ulong> counter = new AtomicCounter<ulong>(0);
		private readonly DealerWorkerConfiguration config;
		private readonly IMessageFactory messageFactory;
		private readonly TimeSpan heartbeatThreshold;

		private WorkerState state;
		private DateTime lastHeartbeat = DateTime.MinValue;

		public string Identity => config.Identity;
		public ISet<string> Services => config.Services;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => false;
		public bool NeedTimer => true;

		public DealerWorkerService(DealerWorkerConfiguration config, IMessageFactory messageFactory, ILogger<DealerWorkerService> logger) {
			this.config = config;
			this.messageFactory = messageFactory;
			this.logger = logger;
			heartbeatThreshold = TimeSpan.FromMilliseconds(config.ActualHeartbeatThreshold);
		}

		public void Init(IMessagingService dealerClient) {
			logger.LogInformation("Starging worker and connecting to broker: {broker}", config.EndPoint);
			var msg = new WorkerConnect(Identity, counter.NextId(), Services);
			dealerClient.SubmitToQueue(msg);
		}

		public bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg) {
			switch (msg) {
				case AAReconnect _:
					dealerClient.Transmit(new WorkerConnect(Identity, counter.NextId(), Services));
					break;
				case BrokerConnectOk _:
					lastHeartbeat = DateTime.UtcNow;
					if (state == WorkerState.Unavailable) {
						state = WorkerState.Connected;
					}
					break;
				case ServerAck _:
					lastHeartbeat = DateTime.UtcNow;
					break;
				case BrokerRequest command:
					lastHeartbeat = DateTime.UtcNow;
					break;
				default:
					return false;
			}
			return true;
		}

		public bool ProcessTransmitQueue(IMessagingService dealerClient, object msg) => false;
		public void ProcessTimerElapsed(DealerClient dealerClient) {
			try {
				if (state != WorkerState.Unavailable) {
					var elapsed = DateTime.UtcNow - lastHeartbeat;
					if (elapsed > heartbeatThreshold) {
						state = WorkerState.Unavailable;
						logger.LogInformation("disconnect: {elapsed:#,#} > {threshold:#,#}", elapsed.TotalMilliseconds, heartbeatThreshold.TotalMilliseconds);
					} else {
						dealerClient.Transmit(new WorkerHeartbeat(Identity, counter.NextId()));
					}
				}
			} catch (Exception err) {
				logger.LogError(err, "error process worker service timer event");
			}
		}
	}
}