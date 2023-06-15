using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Services {
	public interface IRouterServerService {
		bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg);
		bool ProcessTransmitQueue(IMessagingService messagingService, object msg);

		bool CanReceive { get; }
		bool CanTransmit { get; }
		bool NeedTimer { get; }
	}
}
