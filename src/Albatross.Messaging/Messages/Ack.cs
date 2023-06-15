using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack msg from a server socket such as router
	/// </summary>
	public record class Ack : Message, IMessage {
		public static string MessageHeader { get => "ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new Ack(endpoint, id);

		public Ack(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}