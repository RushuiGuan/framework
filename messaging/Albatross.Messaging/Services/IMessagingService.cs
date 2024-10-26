using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using NetMQ;
using System;

namespace Albatross.Messaging.Services {
	/// <summary>
	/// A messaging service that runs a zeromq socket and netmq queue.
	/// </summary>
	public interface IMessagingService {
		IEventWriter EventWriter { get; }
		/// <summary>
		/// A thread safe call to send an object to queue for processing.
		/// </summary>
		/// <param name="message"></param>
		void SubmitToQueue(object message);
		/// <summary>
		/// Transmit the outgoing message via socket.  Not thread safe.  Should only be run on the poller thread.
		/// </summary>
		/// <param name="msg"></param>
		void Transmit(IMessage msg);

		ClientState GetClientState(string identity);

		IAtomicCounter Counter { get; }
	}
}