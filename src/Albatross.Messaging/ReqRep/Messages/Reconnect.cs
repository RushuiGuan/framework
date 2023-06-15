using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "b:reconnect";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => new Reconnect(route, messageId);

		public Reconnect(string route, ulong messageId) : base(MessageHeader, route, messageId) {
			Route = route;
		}
	}
}
