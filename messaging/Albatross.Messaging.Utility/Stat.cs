using Albatross.CommandLine;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albatross.Messaging.Utility {
	[Verb("stat", typeof(Stat))]
	public class StatOptions {
		[Option("s", Description = "The local time to start searching for the events")]
		public DateTime? Start { get; set; }
		[Option("e", Description = "The local time to stop searching for the events")]
		public DateTime? End { get; set; }
		[Option("p", "pattern", Description ="Regular expression pattern that can be used as a filter on the message class name")]
		public string? MessageClassNameFilterPattern { get; set; }
	}

	public class Stat : BaseHandler<StatOptions> {
		private readonly IMessageFactory messageFactory;
		private readonly MessagingGlobalOptions messagingOptions;

		public Stat(IMessageFactory messageFactory, IOptions<MessagingGlobalOptions> messagingOptions, IOptions<StatOptions> options) : base(options) {
			this.messageFactory = messageFactory;
			this.messagingOptions = messagingOptions.Value;
		}

		public override async Task<int> InvokeAsync(InvocationContext context) {
			var start = DateTime.SpecifyKind(options.Start?? DateTime.MinValue, DateTimeKind.Local);
			var end = DateTime.SpecifyKind(options.End ?? DateTime.Now, DateTimeKind.Local);
			var files = messagingOptions.GetEventSourceFiles();
			var filesToSearch = files.FindFilesToSearch(start, end);
			var conversations = new Dictionary<ulong, Conversation>();
			Regex? regex = null;
			if(options.MessageClassNameFilterPattern != null) {
				regex = new Regex(options.MessageClassNameFilterPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			foreach (var file in filesToSearch) {
				using var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using (var reader = new StreamReader(stream)) {
					while (!reader.EndOfStream) {
						var line = await reader.ReadLineAsync();
						if (line != null) {
							if (EventEntry.TryParseLine(messageFactory, line, out var entry)) {
								if (conversations.TryGetValue(entry.Message.Id, out var messageGroup)) {
									messageGroup.Add(entry);
								} else {
									if (entry.TimeStamp >= start && entry.TimeStamp <= end) {
										messageGroup = new Conversation(entry.Message.Route ?? string.Empty, entry.Message.Id);
										conversations.Add(entry.Message.Id, messageGroup);
									}
								}
							}
						}
					}
				}
			}
			return 0;
		}
	}
}