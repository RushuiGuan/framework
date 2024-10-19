using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using System.Collections.Generic;
using System.IO;

namespace Albatross.Messaging.Utility {
	public class MessageGroup {
		public MessageGroup(string client, ulong id) {
			Client = client;
			Id = id;
		}

		public ulong Id { get; set; }
		public string Client { get; set; }
		public string Key => $"{Client}-{Id}";

		public List<EventEntry> Entries { get; set; } = new List<EventEntry>();

		public void Write(TextWriter writer) {
			writer.WriteLine($"Client: {Client}, Id: {Id}");
			foreach (var entry in Entries) {
				entry.Write(writer);
				if (entry.Message is CommandRequest request) {
					writer.WriteLine(request.Payload.ToUtf8String());
				} else if (entry.Message is CommandReply reply) {
					writer.WriteLine(reply.Payload.ToUtf8String());
				} else if (entry.Message is CommandErrorReply error) {
					writer.WriteLine(error.Message.ToUtf8String());
				} else if (entry.Message is CommandRequestError requestError) {
					writer.WriteLine(requestError.Message.ToUtf8String());
				}
			}
		}
	}
}
