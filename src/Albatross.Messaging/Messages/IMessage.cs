using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Messages {
	public interface IMessage {
		virtual static string MessageHeader { get => throw new NotImplementedException(); }

		string? Route { get; }
		string Header { get; }
		ulong Id { get; }

		void ReadFromFrames(NetMQMessage msg);
		void WriteToFrames(NetMQMessage msg);

		void ReadFromText(string line, ref int offset);
		void WriteToText(TextWriter writer);
	}
}