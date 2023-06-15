using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	/// <summary>
	/// Ping request with id
	/// </summary>
	public record class PingRequest : Message, IMessage {
		public static string MessageHeader => "ping-req";
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new PingRequest(endpoint, id);
		public PingRequest(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}