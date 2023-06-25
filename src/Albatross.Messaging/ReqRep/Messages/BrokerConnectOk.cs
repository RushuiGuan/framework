using Albatross.Messaging.Messages;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Broker send back Ok after receiving the Connect msg
	/// </summary>
	public record class BrokerConnectOk : Message, IMessage {
		public static string MessageHeader => "aa_connect_ok";
		
		public double HeartbeatInterval { get; init; }
		public double HeartbeatThreshold { get; init; }


		public BrokerConnectOk(string route, ulong id, double interval, double threshold) : base(MessageHeader, route, id) {
			HeartbeatInterval = interval;
			HeartbeatThreshold = threshold;
		}
	}
}