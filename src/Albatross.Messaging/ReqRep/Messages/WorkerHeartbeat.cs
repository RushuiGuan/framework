using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Heartbeat msg from worker to broker
	/// </summary>
	public record class WorkerHeartbeat : Message, IMessage {
		public static string MessageHeader => "worker-ping";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => new WorkerHeartbeat(route, frames.PopUInt());

		public WorkerHeartbeat(string route, ulong messageId) : base(MessageHeader, route, messageId) {
		}
		public WorkerHeartbeat() { }
	}
}