using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Services {
	public interface IRouterServerService {
		bool ProcessReceivedMsg(IMessagingService routerServer, IMessage msg);
		bool ProcessTransmitQueue(IMessagingService routerServer, object msg);
		void ProcessTimerElapsed(IMessagingService routerServer, ulong count);

		bool CanReceive { get; }
		/// <summary>
		/// When true, the service will need to process queued transmit object other than IMessage
		/// </summary>
		bool HasCustomTransmitObject { get; }
		bool NeedTimer { get; }
	}
}
