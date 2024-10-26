using Albatross.CommandLine;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Albatross.Text;
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

		[Option("s")]
		public DateTime Start { get; set; }

		[Option("e")]
		public DateTime? End { get; set; }
	}
	public class Measure : BaseHandler<Measureoptions> {
		private readonly IMessageFactory messageFactory;

		public Measure(IMessageFactory messageFactory, IOptions<Measureoptions> options, ILogger logger) : base(options, logger) {
			this.messageFactory = messageFactory;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			Dictionary<ulong, MessageGroup> result = new Dictionary<ulong, MessageGroup>();
			if (!string.IsNullOrEmpty(options.Project)) {
				string folder;
				if (string.IsNullOrEmpty(options.ProjectLocation)) {
					folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), options.Project);
				} else {
					folder = System.IO.Path.Combine(options.ProjectLocation, options.Project);
				}
				foreach (var file in System.IO.Directory.EnumerateFiles(folder, "*.log")) {
					await SearchFile(result, file, messageFactory);
				}
			}
			await this.writer.PrintTable(result.Values, new PrintOptionBuilder<PrintTableOption>()
				.Property(nameof(MessageGroup.Id),
					nameof(MessageGroup.Type),
					nameof(MessageGroup.Mode),
					nameof(MessageGroup.RequestAckDuration),
					nameof(MessageGroup.ReplyDuration),
					nameof(MessageGroup.ReplyAckDuration),
					nameof(MessageGroup.Sequence))
				.Build());
			return 0;
		}

		async Task SearchFile(Dictionary<ulong, MessageGroup> dict, string file, IMessageFactory messageFactory) {
			using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			using (var reader = new StreamReader(stream)) {
				while (!reader.EndOfStream) {
					var line = await reader.ReadLineAsync();
					if (line != null) {
						if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
							if (dict.TryGetValue(entry.Message.Id, out var messageGroup)) {
								messageGroup.Add(entry);
							} else {
								if (entry.TimeStamp >= options.Start && (options.End == null || entry.TimeStamp <= options.End.Value)) {
									messageGroup = new MessageGroup(entry.Message.Route ?? string.Empty, entry.Message.Id);
									dict.Add(entry.Message.Id, messageGroup);
								}
							}
						}
					}
				}
			}
		}
	}
}