using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Services {
	public interface IDealerClientService {
		bool AcceptMessage(IMessage msg);
		bool ProcessTransmitQueueItem(object msg);
		void SetMessagingService(IMessagingService messagingService);
	}
}
