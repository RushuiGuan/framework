using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class NoAvailableWorker : Message, IMessage {
		public static string MessageHeader => "noworker";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			string? service = frames.Any() ? frames.PopUtf8String() : null;
			return new NoAvailableWorker(route, messageId, service);
		}

		public string? Service { get; init; }


		public NoAvailableWorker(string route, ulong messageId, string? service) : base(MessageHeader, route, messageId) {
			Service = service;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(Service);
			return msg;
		}
	}
}