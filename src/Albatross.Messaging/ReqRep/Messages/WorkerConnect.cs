using Albatross.Messaging.Messages;
using NetMQ;
using System.Collections.Generic;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Messages for worker to connect to broker
	/// </summary>
	public record class WorkerConnect : Message, IMessage {
		public static string MessageHeader => "worker-connect";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			var services = new HashSet<string>();
			foreach (var item in frames) {
				services.Add(item.Buffer.ToUtf8String());
			}
			return new WorkerConnect(route, messageId, services);
		}

		public ISet<string> Services { get; } = new HashSet<string>();

		public WorkerConnect(string route, ulong messageId, ISet<string> services) : base(MessageHeader, route, messageId) {
			Services = services;
		}
		public WorkerConnect	() { }
	}
}