using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Broker send back Ok after receiving the Connect msg
	/// </summary>
	public record class BrokerConnectOk : Message, IMessage {
		public static string MessageHeader => "b:ok";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames)
			=> new BrokerConnectOk(route, messageId, frames.PopDouble(), frames.PopDouble());

		public double HeartbeatInterval { get; init; }
		public double HeartbeatThreshold { get; init; }


		public BrokerConnectOk(string route, ulong id, double interval, double threshold) : base(MessageHeader, route, id) {
			HeartbeatInterval = interval;
			HeartbeatThreshold = threshold;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.Append(BitConverter.GetBytes(HeartbeatInterval));
			msg.Append(BitConverter.GetBytes(HeartbeatThreshold));
			return msg;
		}
	}
}