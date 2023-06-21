﻿using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack msg from a server socket such as router
	/// </summary>
	public record class Heartbeat : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new Heartbeat(endpoint, id);

		public Heartbeat(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}