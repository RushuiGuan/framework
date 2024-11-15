using Albatross.CommandLine;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("seek", typeof(Seek))]
	public class Seekoptions {
		[Option("i", "id")]
		public ulong? Id { get; set; }
	}
	public class Seek : BaseHandler<Seekoptions> {
		private readonly IMessageFactory messageFactory;
		private readonly MessagingGlobalOptions messagingOptions;

		public Seek(IMessageFactory messageFactory, IOptions<MessagingGlobalOptions> messagingOptions, IOptions<Seekoptions> options, ILogger logger) : base(options, logger) {
			this.messageFactory = messageFactory;
			this.messagingOptions = messagingOptions.Value;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			MessageGroup? message = null;
			string folder;
			if (string.IsNullOrEmpty(messagingOptions.EventSourceFolder)) {
				folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), messagingOptions.Application);
			} else {
				folder = messagingOptions.EventSourceFolder;
			}
			foreach (var file in System.IO.Directory.EnumerateFiles(folder, "*.log")) {
				this.writer.WriteLine($"Searching file {file}");
				message = await SearchFile(message, file, messageFactory);
			}
			if (message != null) {
				await message.Write(Console.Out);
			}
			return 0;
		}

		async Task<MessageGroup?> SearchFile(MessageGroup? message, string file, IMessageFactory messageFactory) {
			using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(stream)) {
				while (!reader.EndOfStream) {
					var line = await reader.ReadLineAsync();
					if (line != null) {
						if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
							if (options.Id == entry.Message.Id) {
								if (message == null) {
									message = new MessageGroup(entry.Message.Route ?? string.Empty, entry.Message.Id);
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