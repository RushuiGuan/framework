using Albatross.Messaging.Messages;
using System;

namespace Albatross.Messaging.Services {
	public interface IDealerClientService {
		bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg);
		bool ProcessTransmitQueue(IMessagingService dealerClient, object msg);

		bool CanReceive { get; }
		bool CanTransmit { get; }
	}
}
