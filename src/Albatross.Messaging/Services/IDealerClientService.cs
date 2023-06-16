using Albatross.Messaging.Messages;
using System;

namespace Albatross.Messaging.Services {
	public interface IDealerClientService {
		bool ProcessReceivedMsg(DealerClient dealerClient, IMessage msg);
		bool ProcessTransmitQueue(DealerClient dealerClient, object msg);

		bool CanReceive { get; }
		bool CanTransmit { get; }
	}
}
