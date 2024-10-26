using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Services {
	public interface IRouterServerService {
		/// <summary>
		/// Invoked when the router server receives an <see cref="IMessage"/> from its socket.  This method will always be invoked using the netmq's main thread
		/// </summary>
		/// <param name="routerServer"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool ProcessReceivedMsg(IMessagingService routerServer, IMessage msg);
		/// <summary>
		/// Invoked when the messaging server is ready to process its queue.  The items in the queue will always be processed on the netmq's main thread
		/// </summary>
		/// <param name="routerServer"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		bool ProcessQueue(IMessagingService routerServer, object msg);
		/// <summary>
		/// Invoked when the router server's timer elapsed.  This method will always be invoked using the netmq's main thread
		/// </summary>
		/// <param name="routerServer"></param>
		/// <param name="count"></param>
		void ProcessTimerElapsed(IMessagingService routerServer, ulong count);

		bool CanReceive { get; }
		/// <summary>
		/// When true, the service will need to process queued transmit object other than IMessage
		/// </summary>
		bool HasCustomTransmitObject { get; }
		bool NeedTimer { get; }
	}
}