using Albatross.Collections;
using Albatross.CommandLine;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("measure", typeof(Measure))]
	public class Measureoptions {
		[Option("p")]
		public string Project { get; set; } = string.Empty;

		[Option("l")]
		public string? ProjectLocation { get; set; }

		[Option("i", "id")]
		public ulong Id { get; set; }
	}
	public class Measure : BaseHandler<Measureoptions> {
		private readonly IMessageFactory messageFactory;

		public Measure(IMessageFactory messageFactory, IOptions<Measureoptions> options, ILogger logger) : base(options, logger) {
			this.messageFactory = messageFactory;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			List<MessageGroup> result = new List<MessageGroup>();
			if (!string.IsNullOrEmpty(options.Project)) {
				string folder;
				if (string.IsNullOrEmpty(options.ProjectLocation)) {
					folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Project);
				} else {
					folder = System.IO.Path.Combine(options.ProjectLocation, options.Project);
				}
				foreach (var file in System.IO.Directory.EnumerateFiles(folder, "*.log")) {
					var msg = await SearchFile(file, messageFactory);
					result.AddIfNotNull(msg);
				}
			}
			foreach (var group in result) {
				await group.Write(Console.Out);
			}
			return 0;
		}

		async Task<MessageGroup?> SearchFile(string file, IMessageFactory messageFactory) {
			MessageGroup? message = null;
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
