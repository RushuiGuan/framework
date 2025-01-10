using Albatross.CommandLine;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("seek", typeof(Seek), Description = "Search the event source files for a conversation with the specified id")]
	public class Seekoptions {
		[Option("i", "id")]
		public ulong? Id { get; set; }

		[Option("s", Description = "The local time to start searching for the events")]
		public DateTime? Start { get; set; }

		[Option("e", Description = "The local time to stop searching for the events")]
		public DateTime? End { get; set; }

		[Option("p", "pattern", Description = "Regular expression pattern that can be used as a filter on the message class name")]
		public string? MessageClassNameFilterPattern { get; set; }
	}
	public class Seek : BaseHandler<Seekoptions> {
		private readonly IMessageFactory messageFactory;
		private readonly MessagingGlobalOptions messagingOptions;

		public Seek(IMessageFactory messageFactory, IOptions<MessagingGlobalOptions> messagingOptions, IOptions<Seekoptions> options) : base(options) {
			this.messageFactory = messageFactory;
			this.messagingOptions = messagingOptions.Value;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			Conversation? conversation = null;
			foreach (var file in messagingOptions.GetEventSourceFiles()) {
				this.writer.WriteLine($"Searching file {file}");
				conversation = await SearchFile(conversation, file, messageFactory);
			}
			if (conversation != null) {
				await conversation.Write(Console.Out);
			}
			return 0;
		}

		async Task<Conversation?> SearchFile(Conversation? message, FileInfo file, IMessageFactory messageFactory) {
			using var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(stream)) {
				while (!reader.EndOfStream) {
					var line = await reader.ReadLineAsync();
					if (line != null) {
						if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
							if (options.Id == entry.Message.Id) {
								if (message == null) {
									message = new Conversation(entry.Message.Route ?? string.Empty, entry.Message.Id);
								}
								message.Add(entry);
							}
						}
					}
				}
			}
			return message;
		}
	}
}