using Albatross.Messaging.Messages;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Broker send back Ok after receiving the Connect msg
	/// </summary>
	public record class BrokerConnected : Message, IMessage {
		public static string MessageHeader => "broker-connected";

		public BrokerConnected(string route, ulong id) : base(MessageHeader, route, id) {
		}
		public BrokerConnected() { }
	}
}