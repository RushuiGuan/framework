using Albatross.Messaging.Messages;
using System;

namespace Albatross.Messaging.Services {
	public interface IDealerClientService {
		/// <summary>
		/// the init method is called prior the poller starts running.  This method is for protocol connectivity 
		/// related initialization.  Use SubmitToQueue to queue up messages.  Do not call transmit directly.
		/// </summary>
		/// <param name="dealerClient"></param>
		void Init(IMessagingService dealerClient);
		bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg);
		bool ProcessQueue(IMessagingService dealerClient, object msg);
		void ProcessTimerElapsed(IMessagingService dealerClient, ulong counter);

		bool CanReceive { get; }
		/// <summary>
		/// When true, the service will need to process queued transmit object other than IMessage
		/// </summary>
		bool HasCustomTransmitObject { get; }
		bool NeedTimer { get; }
	}
}