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
		[Option("p")]
		public string Project { get; set; } = string.Empty;

		[Option("l")]
		public string? ProjectLocation { get; set; }

		[Option("i", "id")]
		public ulong? Id { get; set; }
	}
	public class Seek : BaseHandler<Seekoptions> {
		private readonly IMessageFactory messageFactory;

		public Seek(IMessageFactory messageFactory, IOptions<Seekoptions> options, ILogger logger) : base(options, logger) {
			this.messageFactory = messageFactory;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			MessageGroup? message = null;
			if (!string.IsNullOrEmpty(options.Project)) {
				string folder;
				if (string.IsNullOrEmpty(options.ProjectLocation)) {
					folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Project);
				} else {
					folder = System.IO.Path.Combine(options.ProjectLocation, options.Project);
				}
				foreach (var file in System.IO.Directory.EnumerateFiles(folder, "*.log")) {
					message = await SearchFile(message, file, messageFactory);
				}
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