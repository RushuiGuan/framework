using Albatross.Messaging.Messages;
using System;

namespace Albatross.Messaging.Services {
	public interface IDealerClientService {
		void Init(IMessagingService dealerClient);
		bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg);
		bool ProcessTransmitQueue(IMessagingService dealerClient, object msg);
		void ProcessTimerElapsed(DealerClient dealerClient);

		bool CanReceive { get; }
		/// <summary>
		/// When true, the service will need to process queued transmit object other than IMessage
		/// </summary>
		bool HasCustomTransmitObject { get; }
		bool NeedTimer { get; }
	}
}
