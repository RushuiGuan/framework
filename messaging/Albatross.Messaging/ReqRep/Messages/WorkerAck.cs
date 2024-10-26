using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class WorkerAck : Message, IMessage {
		public static string MessageHeader => "worker-ack";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => new WorkerAck(route, messageId);

		public WorkerAck(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
		public WorkerAck() { }
	}
}