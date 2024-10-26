using Albatross.Messaging.Messages;
using Albatross.Messaging.ReqRep.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Messaging.ReqRep {
	public class DealerWorkerService : IDealerClientService {
		private readonly ILogger<DealerWorkerService> logger;
		private WorkerState state;
		private DateTime lastHeartbeat = DateTime.MinValue;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => false;
		public bool NeedTimer => true;

		public DealerWorkerService(ILogger<DealerWorkerService> logger) {
			this.logger = logger;
		}

		public void Init(IMessagingService dealerClient) {
			dealerClient.SubmitToQueue(new Connect(string.Empty, dealerClient.Counter.NextId()));
		}

		public bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg) {
			switch (msg) {
				case BrokerRequest command:
					lastHeartbeat = DateTime.UtcNow;
					break;
				default:
					return false;
			}
			return true;
		}
		public bool ProcessQueue(IMessagingService dealerClient, object msg) => false;
		public void ProcessTimerElapsed(IMessagingService dealerClient, ulong counter) {
			//try {
			//	if (state != WorkerState.Unavailable) {
			//		var elapsed = DateTime.UtcNow - lastHeartbeat;
			//		if (elapsed > heartbeatThreshold) {
			//			state = WorkerState.Unavailable;
			//			logger.LogInformation("disconnect: {elapsed:#,#} > {threshold:#,#}", elapsed.TotalMilliseconds, heartbeatThreshold.TotalMilliseconds);
			//		} else {
			//			dealerClient.Transmit(new Heartbeat(Identity, this.counter.NextId()));
			//		}
			//	}
			//} catch (Exception err) {
			//	logger.LogError(err, "error process worker service timer event");
			//}
		}
	}
}